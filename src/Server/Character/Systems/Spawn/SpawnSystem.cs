using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Character.Systems.Selection;
using Server.Database;

namespace Server.Character.Systems.Spawn;

public sealed class SpawnSystem : ISystem
{
    private readonly IDbContextFactory<ServerDbContext> contextFactory;
    private readonly ISpawnRequestSelectionEvent spawnRequestSelectionEvent;

    public SpawnSystem(IDbContextFactory<ServerDbContext> contextFactory,
        ISelectionRequestSpawnEvent selectionRequestSpawnEvent, ISpawnRequestSelectionEvent spawnRequestSelectionEvent)
    {
        this.contextFactory = contextFactory;
        this.spawnRequestSelectionEvent = spawnRequestSelectionEvent;
        selectionRequestSpawnEvent.AddHandler(OnRequestSpawn);
    }

    private async Task OnRequestSpawn(Player player, long id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var model = await context
            .Characters
            .Where(model => model.Id == id)
            .Select(model => new { model.Name, model.X, model.Y, model.Z, model.A, model.World, model.Interior, model.Skin })
            .FirstOrDefaultAsync();

        if (model is not null)
        {
            player.ToggleSpectating(false);
            player.SetSpawnInfo(0, model.Skin, new Vector3(model.X, model.Y, model.Z), model.A);
            player.Spawn();
            player.PutCameraBehindPlayer();

            player.Name = model.Name;
            player.Skin = model.Skin;
            player.VirtualWorld = model.World;
            player.Interior = model.Interior;
            return;
        }
        else
        {
            await spawnRequestSelectionEvent.InvokeAsync(player);
        }
    }
}
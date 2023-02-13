using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Components;
using Server.Account.Systems.Login;
using Server.Account.Systems.SignUp;
using Server.Character.Systems.Creation;
using Server.Character.Systems.Spawn;
using Server.Database;

namespace Server.Character.Systems.Selection;

public sealed class SelectionSystem : ISystem
{
    private readonly IDbContextFactory<ServerDbContext> contextFactory;
    private readonly IDialogService dialogService;
    private readonly ISelectionRequestSpawnEvent selectionRequestSpawnEvent;
    private readonly ISelectionRequestSetupEvent selectionRequestSetupEvent;
    private readonly ISpawnRequestSelectionEvent spawnRequestSelectionEvent;

    public SelectionSystem(ILoginEvent loginEvent, IDbContextFactory<ServerDbContext> contextFactory,
        IDialogService dialogService, ISelectionRequestSpawnEvent selectionRequestSpawnEvent,
        ISelectionRequestSetupEvent selectionRequestSetupEvent, ICreationFinishedEvent creationFinishedEvent,
        ISignedUpEvent signedUpEvent, ISpawnRequestSelectionEvent spawnRequestSelectionEvent)
    {
        this.contextFactory = contextFactory;
        this.dialogService = dialogService;
        this.selectionRequestSpawnEvent = selectionRequestSpawnEvent;
        this.selectionRequestSetupEvent = selectionRequestSetupEvent;
        this.spawnRequestSelectionEvent = spawnRequestSelectionEvent;
        loginEvent.AddHandler(OnLoggedIn);
        creationFinishedEvent.AddHandler(OnLoggedIn);
        signedUpEvent.AddHandler(OnLoggedIn);
        spawnRequestSelectionEvent.AddHandler(OnLoggedIn);
    }

    private class CharacterSelectionData
    {
        public long Id;
        public string Name = string.Empty;
        public int Age;
    }

    public async Task OnLoggedIn(Player player)
    {
        var account = player.GetComponent<AccountComponent>();
        if (account is null)
        {
            return;
        }

        await using var context = await contextFactory.CreateDbContextAsync();

        var characters = await context
            .Characters
            .Where(model => model.AccountId == account.Id)
            .Select(model => new CharacterSelectionData()
            {
                Id = model.Id,
                Name = model.Name,
                Age = model.Age
            })
            .Take(3)
            .ToArrayAsync();

        var selectionDialog =
            new TablistDialog("Nhan vat", "Lua chon", "Thoat", new[] { "Nhan vat ban se tham gia la?", "" });

        foreach (var character in characters)
        {
            selectionDialog.Add(new[] { character.Name, character.Age.ToString() + " tuoi" }, character);
        }

        if (characters.Length < 3)
        {
            selectionDialog.Add(new[] { "{D1D1D1}(+) Tao nhan vat moi", "" });
        }

        await OnSelectionDialogResponse(player, await dialogService.Show(player, selectionDialog));
    }

    private async Task OnSelectionDialogResponse(Player player, TablistDialogResponse response)
    {
        if (response.Response == DialogResponse.LeftButton)
        {
            if (response.Item.Tag is CharacterSelectionData data)
            {
                await selectionRequestSpawnEvent.InvokeAsync(player, data.Id);
            }
            else
            {
                await selectionRequestSetupEvent.InvokeAsync(player);
            }
        }
        else
        {
            player.Kick();
        }
    }
}
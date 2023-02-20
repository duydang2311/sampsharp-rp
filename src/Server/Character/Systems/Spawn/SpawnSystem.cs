using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Systems.Authentication;
using Server.Character.Components;
using Server.Character.Systems.Selection;
using Server.Chat.Components;
using Server.Database;

namespace Server.Character.Systems.Spawn;

public sealed class SpawnSystem : ISystem
{
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly IAuthenticationSystem authenticationSystem;
	private readonly ICharacterSpawnedEvent characterSpawnedEvent;

	public SpawnSystem(ICharacterSelectedEvent characterSelectedEvent, IDbContextFactory<ServerDbContext> dbContextFactory, IAuthenticationSystem authenticationSystem, ICharacterSpawnedEvent characterSpawnedEvent)
	{
		this.dbContextFactory = dbContextFactory;
		this.authenticationSystem = authenticationSystem;
		this.characterSpawnedEvent = characterSpawnedEvent;

		characterSelectedEvent.AddHandler((player, id, e) =>
		{
			if (id == -1)
			{
				return Task.CompletedTask;
			}
			e.Cancel = true;
			return SpawnCharacter(player, id);
		});
	}

	private async Task SpawnCharacter(Player player, long id)
	{
		await using var ctx = await dbContextFactory.CreateDbContextAsync().ConfigureAwait(false);
		var model = await ctx.Characters
			.Where(m => m.Id == id)
			.Select(m => new { m.Skin, m.X, m.Y, m.Z, m.A, m.Interior, m.World, m.Health })
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);

		if (model is null)
		{
			_ = authenticationSystem.AuthenticateAsync(player);
			return;
		}

		player.AddComponent(new CharacterComponent { Id = id });
		player.AddComponent(new PermissionComponent(PermissionLevel.Player));
		player.ToggleSpectating(false);
		player.SetSpawnInfo(255, model.Skin, new Vector3(model.X, model.Y, model.Z), model.A);
		player.Spawn();
		player.Health = model.Health;
		await characterSpawnedEvent.InvokeAsync(player).ConfigureAwait(false);
	}

	[Event]
	private async void OnPlayerDisconnect(Player player, int reason)
	{
		var component = player.GetComponent<CharacterComponent>();
		if (component is null)
		{
			return;
		}

		var position = player.Position;
		var angle = player.Angle;
		var world = player.VirtualWorld;
		var interior = player.Interior;
		var health = player.Health;

		await using var ctx = await dbContextFactory
			.CreateDbContextAsync()
			.ConfigureAwait(false);
		await ctx.Characters
			.Where(m => m.Id == component.Id)
			.ExecuteUpdateAsync(m => m
				.SetProperty(m => m.X, position.X)
				.SetProperty(m => m.Y, position.Y)
				.SetProperty(m => m.Z, position.Z)
				.SetProperty(m => m.A, angle)
				.SetProperty(m => m.World, world)
				.SetProperty(m => m.Interior, interior)
				.SetProperty(m => m.Health, health))
			.ConfigureAwait(false);
	}
}

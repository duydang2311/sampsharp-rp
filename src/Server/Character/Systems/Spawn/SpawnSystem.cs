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
		using var ctx = dbContextFactory.CreateDbContext();
		var model = await ctx.Characters
			.Where(m => m.Id == id)
			.Select(m => new
			{
				m.Skin,
				m.X,
				m.Y,
				m.Z,
				m.A,
				m.Interior,
				m.World,
				m.Health,
				m.PermissionLevel
			})
			.FirstOrDefaultAsync();

		if (model is null)
		{
			_ = authenticationSystem.AuthenticateAsync(player);
			return;
		}

		player.AddComponent(new CharacterComponent { Id = id });
		player.AddComponent(new PermissionComponent(model.PermissionLevel));
		player.ToggleSpectating(false);
		player.SetSpawnInfo(255, model.Skin, new Vector3(model.X, model.Y, model.Z), model.A);
		player.Spawn();
		player.Health = model.Health;
		player.Skin = model.Skin;
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

		using var ctx = dbContextFactory.CreateDbContext();
		await ctx.Characters
			.Where(m => m.Id == component.Id)
			.ExecuteUpdateAsync(m => m
				.SetProperty(m => m.X, player.Position.X)
				.SetProperty(m => m.Y, player.Position.Y)
				.SetProperty(m => m.Z, player.Position.Z)
				.SetProperty(m => m.A, player.Angle)
				.SetProperty(m => m.World, player.VirtualWorld)
				.SetProperty(m => m.Interior, player.Interior)
				.SetProperty(m => m.Health, player.Health))
			.ConfigureAwait(false);
	}
}

using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Character.Systems.EnterCommand;
using Server.Chat.Components;
using Server.Database;
using Server.Door.Components;
using Server.Door.Entities;
using Server.Door.Services;

namespace Server.Door.Systems.Enter;

public sealed partial class EnterSystem : ISystem
{
	private readonly IDoorFactory doorFactory;
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly ILogger<EnterSystem> logger;

	[Event]
	private void OnGameModeInit()
	{
		using var ctx = dbContextFactory.CreateDbContext();
		ctx.Database.EnsureDeleted();
		ctx.Database.EnsureCreated();
	}

	[Event]
	private void OnPlayerRequestSpawn(Player player)
	{
		player.Spawn();
		player.Position = new Vector3(0, 0, 3);
		player.AddComponent<PermissionComponent>(PermissionLevel.Player | PermissionLevel.Admin);
	}

	public EnterSystem(IEnterCommandEvent enterCommandEvent, IDoorFactory doorFactory, IDbContextFactory<ServerDbContext> dbContextFactory, ILogger<EnterSystem> logger)
	{
		this.doorFactory = doorFactory;
		this.dbContextFactory = dbContextFactory;
		this.logger = logger;

		enterCommandEvent.AddHandler(OnEnterCommandEvent);
	}

	[LoggerMessage(
		EventId = 0,
		Level = LogLevel.Warning,
		Message = "Trying to enter Door#{Id} that is not existed in database")]
	public partial void LogDoorModelMissing(long id);

	private void OnEnterCommandEvent(Player player, CancelEventArgs e)
	{
		float distanceSquared;
		var minDistanceSquared = (2f * 2f) + 1e-7;
		IDoorInteraction? closestExitInteraction = null;
		foreach (var interaction in doorFactory.Grid.FindComponents(player.Position.XY, 2).Cast<IDoorInteraction>())
		{
			if (interaction.Door is not ILogicalDoor logicalDoor
			|| logicalDoor.EntranceInteraction is null
			|| logicalDoor.ExitInteraction is null
			|| logicalDoor.EntranceInteraction != interaction)
			{
				continue;
			}

			distanceSquared = Vector3.DistanceSquared(interaction.Position, player.Position);
			if (distanceSquared < minDistanceSquared)
			{
				closestExitInteraction = logicalDoor.ExitInteraction;
				minDistanceSquared = distanceSquared;
			}
		}
		if (closestExitInteraction is null)
		{
			return;
		}

		e.Cancel = true;
		player.Position = closestExitInteraction.Position;
		player.VirtualWorld = closestExitInteraction.World;
		player.Interior = closestExitInteraction.Interior;
	}
}

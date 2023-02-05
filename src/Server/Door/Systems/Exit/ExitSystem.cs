using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Character.Systems.ExitCommand;
using Server.Database;
using Server.Door.Components;
using Server.Door.Entities;
using Server.Door.Services;

namespace Server.Door.Systems.Exit;

public sealed partial class ExitSystem : ISystem
{
	private readonly IDoorFactory doorFactory;
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly ILogger<ExitSystem> logger;

	public ExitSystem(IExitCommandEvent enterCommandEvent, IDoorFactory doorFactory, IDbContextFactory<ServerDbContext> dbContextFactory, ILogger<ExitSystem> logger)
	{
		this.doorFactory = doorFactory;
		this.dbContextFactory = dbContextFactory;
		this.logger = logger;

		enterCommandEvent.AddHandler(OnExitCommandEvent);
	}

	[LoggerMessage(
		EventId = 0,
		Level = LogLevel.Warning,
		Message = "Trying to exit Door#{Id} that is not existed in database")]
	public partial void LogDoorModelMissing(long id);

	private void OnExitCommandEvent(Player player, CancelEventArgs e)
	{
		float distanceSquared;
		var minDistanceSquared = (2f * 2f) + 1e-7;
		IDoorInteraction? closestEntranceInteraction = null;
		foreach (var interaction in doorFactory.Grid.FindComponents(player.Position.XY, 2).Cast<IDoorInteraction>())
		{
			if (interaction.Door is not ILogicalDoor logicalDoor
			|| logicalDoor.ExitInteraction is null
			|| logicalDoor.EntranceInteraction is null
			|| logicalDoor.ExitInteraction != interaction)
			{
				continue;
			}

			distanceSquared = Vector3.DistanceSquared(interaction.Position, player.Position);
			if (distanceSquared < minDistanceSquared)
			{
				closestEntranceInteraction = logicalDoor.EntranceInteraction;
				minDistanceSquared = distanceSquared;
			}
		}
		if (closestEntranceInteraction is null)
		{
			return;
		}

		e.Cancel = true;
		player.Position = closestEntranceInteraction.Position;
		player.VirtualWorld = closestEntranceInteraction.World;
		player.Interior = closestEntranceInteraction.Interior;
	}
}

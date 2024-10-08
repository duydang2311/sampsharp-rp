using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Character.Systems.EnterCommand;
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
		IDoorInteraction? closestInteraction = null;
		foreach (var interaction in doorFactory.Grid.FindComponents(player.Position.XY, 2).Cast<IDoorInteraction>())
		{
			if (interaction.Door is not ILogicalDoor logicalDoor
			|| logicalDoor.EntranceInteraction is null
			|| logicalDoor.ExitInteraction is null
			|| !interaction.Area.Contains(player.Position.XY))
			{
				continue;
			}

			distanceSquared = Vector3.DistanceSquared(new Vector3(interaction.Area.Center, interaction.Z), player.Position);
			if (distanceSquared < minDistanceSquared)
			{
				closestInteraction = (logicalDoor.EntranceInteraction == interaction)
					? logicalDoor.ExitInteraction
					: logicalDoor.EntranceInteraction;
				minDistanceSquared = distanceSquared;
			}
		}
		if (closestInteraction is null)
		{
			return;
		}

		e.Cancel = true;
		player.Position = new Vector3(closestInteraction.Area.Center, closestInteraction.Z);
		player.VirtualWorld = closestInteraction.World;
		player.Interior = closestInteraction.Interior;
		player.Angle = closestInteraction.Angle;
	}
}

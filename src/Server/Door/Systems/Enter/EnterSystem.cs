using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Character.Systems.EnterCommand;
using Server.Database;
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

	private async Task OnEnterCommandEvent(Player player, CancelEventArgs e)
	{
		float distanceSquared;
		var minDistanceSquared = (3f * 3f) + 1e-7;
		ILogicalDoor? closestDoor = null;
		foreach (var door in doorFactory.Doors)
		{
			if (door is not ILogicalDoor logicalDoor
			|| logicalDoor.EntranceCheckpoint is null
			|| logicalDoor.ExitCheckpoint is null)
			{
				continue;
			}
			distanceSquared = Vector3.DistanceSquared(player.Position, logicalDoor.EntranceCheckpoint.Position);
			if (distanceSquared < minDistanceSquared)
			{
				closestDoor = logicalDoor;
				minDistanceSquared = distanceSquared;
			}
		}
		if (closestDoor is null)
		{
			return;
		}
		e.Cancel = true;

		await using var ctx = await dbContextFactory.CreateDbContextAsync();
		var model = await ctx.Doors
			.Where(m => m.Id == closestDoor.Id)
			.Select(m => new { m.ExitX, m.ExitY, m.ExitZ, m.ExitWorld, m.ExitInterior })
			.AsNoTracking()
			.FirstOrDefaultAsync();
		if (model is null)
		{
			LogDoorModelMissing(closestDoor.Id);
			return;
		}

		player.Position = new Vector3(model.ExitX, model.ExitY, model.ExitZ);
		player.VirtualWorld = model.ExitWorld;
		player.Interior = model.ExitInterior;
	}
}

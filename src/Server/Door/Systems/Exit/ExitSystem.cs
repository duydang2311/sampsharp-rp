using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Character.Systems.ExitCommand;
using Server.Database;
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

	private async Task OnExitCommandEvent(Player player, CancelEventArgs e)
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
			distanceSquared = Vector3.DistanceSquared(player.Position, logicalDoor.ExitCheckpoint.Position);
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
			.Select(m => new { m.EntranceX, m.EntranceY, m.EntranceZ, m.EntranceWorld, m.EntranceInterior })
			.AsNoTracking()
			.FirstOrDefaultAsync();
		if (model is null)
		{
			LogDoorModelMissing(closestDoor.Id);
			return;
		}

		player.Position = new Vector3(model.EntranceX, model.EntranceY, model.EntranceZ);
		player.VirtualWorld = model.EntranceWorld;
		player.Interior = model.EntranceInterior;
	}
}

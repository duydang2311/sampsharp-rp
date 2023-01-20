using Microsoft.Extensions.Logging;
using SampSharp.Streamer.Entities;
using Server.Door.Entities;

namespace Server.Door.Services;

public sealed partial class DoorFactory : IDoorFactory
{
	private readonly IStreamerService streamerService;
	private readonly ILogger<DoorFactory> logger;

	[LoggerMessage(
		EventId = 0,
		Level = LogLevel.Warning,
		Message = "A door of type {Type} was created with invalid Id {Id}")]
	public partial void LogInvalidDoorCreation(Type type, long id);

	public DoorFactory(IStreamerService streamerService, ILogger<DoorFactory> logger)
	{
		this.streamerService = streamerService;
		this.logger = logger;
	}

	public ILogicalDoor CreateLogicalDoor(Action<ILogicalDoor, IStreamerService> doorAction)
	{
		var door = new LogicalDoor();
		doorAction(door, streamerService);
		if (door.Id == 0)
		{
			LogInvalidDoorCreation(typeof(ILogicalDoor), door.Id);
		}
		return door;
	}

	public IPhysicalDoor CreatePhysicalDoor(Action<IPhysicalDoor, IStreamerService> doorAction)
	{
		var door = new PhysicalDoor();
		doorAction(door, streamerService);
		if (door.Id == 0)
		{
			LogInvalidDoorCreation(typeof(IPhysicalDoor), door.Id);
		}
		return door;
	}
}

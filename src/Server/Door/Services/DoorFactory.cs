using Microsoft.Extensions.Logging;
using SampSharp.Streamer.Entities;
using Server.Door.Entities;

namespace Server.Door.Services;

public sealed partial class DoorFactory : IDoorFactory
{
	private readonly IStreamerService streamerService;
	private readonly ILogger<DoorFactory> logger;
	private readonly IDictionary<long, IDoor> doorDictionary = new Dictionary<long, IDoor>();

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
		else
		{
			doorDictionary.Add(door.Id, door);
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
		else
		{
			doorDictionary.Add(door.Id, door);
		}
		return door;
	}

	public bool DestroyDoor(IDoor door)
	{
		switch (door)
		{
			case ILogicalDoor logicalDoor:
				{
					logicalDoor.EntranceCheckpoint?.DestroyEntity();
					logicalDoor.ExitCheckpoint?.DestroyEntity();
					break;
				}
			case IPhysicalDoor physicalDoor:
				{
					physicalDoor.Object?.DestroyEntity();
					break;
				}
			default:
				break;
		}
		return doorDictionary.Remove(door.Id);
	}

	public bool DestroyDoor(long id)
	{
		return doorDictionary.TryGetValue(id, out var door) && DestroyDoor(door);
	}
}

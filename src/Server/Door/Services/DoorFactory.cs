using Microsoft.Extensions.Logging;
using SampSharp.Entities.SAMP;
using SampSharp.Streamer.Entities;
using Server.Door.Components;
using Server.Door.Entities;
using Server.SpatialGrid.Entities;

namespace Server.Door.Services;

public sealed partial class DoorFactory : IDoorFactory
{
	private readonly IStreamerService streamerService;
	private readonly ILogger<DoorFactory> logger;
	private readonly IDictionary<long, IDoor> doorDictionary = new Dictionary<long, IDoor>();

	public IEnumerable<IDoor> Doors => doorDictionary.Values;
	public IGrid Grid { get; }

	[LoggerMessage(
		EventId = 0,
		Level = LogLevel.Warning,
		Message = "A door of type {Type} was created with invalid Id {Id}")]
	public partial void LogInvalidDoorCreation(Type type, long id);

	public DoorFactory(IStreamerService streamerService, ILogger<DoorFactory> logger, ISanAndreasGrid sanAndreasGrid)
	{
		this.streamerService = streamerService;
		this.logger = logger;
		Grid = sanAndreasGrid;
	}

	public ILogicalDoor CreateLogicalDoor(long id)
	{
		var door = new LogicalDoor(id);
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

	public IPhysicalDoor CreatePhysicalDoor(long id)
	{
		var door = new PhysicalDoor(id);
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
			case IPhysicalDoor physicalDoor:
				{
					physicalDoor.Object?.DestroyEntity();
					break;
				}
		}
		return doorDictionary.Remove(door.Id);
	}

	public bool DestroyDoor(long id)
	{
		return doorDictionary.TryGetValue(id, out var door) && DestroyDoor(door);
	}

	public IDoor? FindOne(Predicate<IDoor> filter)
	{
		foreach (var (_, door) in doorDictionary)
		{
			if (filter(door))
			{
				return door;
			}
		}
		return default;
	}

	public IEnumerable<IDoor> FindMany(Predicate<IDoor> filter)
	{
		var doors = new LinkedList<IDoor>();
		foreach (var (_, door) in doorDictionary)
		{
			if (filter(door))
			{
				doors.AddLast(door);
			}
		}
		return doors;
	}

	public IDoorInteraction CreateDoorInteraction(IDoor door, Vector3 position, int world, int interior)
	{
		var interaction = new DoorInteraction(door, position.X, position.Y, position.Z, world, interior);
		Grid.Add(interaction);
		return interaction;
	}

	public bool TryGetDoor(long id, out IDoor door)
	{
		return doorDictionary.TryGetValue(id, out door!);
	}
}

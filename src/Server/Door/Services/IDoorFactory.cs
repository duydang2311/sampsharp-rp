using SampSharp.Streamer.Entities;
using Server.Door.Entities;

namespace Server.Door.Services;

public interface IDoorFactory
{
	IEnumerable<IDoor> Doors { get; }
	ILogicalDoor CreateLogicalDoor(Action<ILogicalDoor, IStreamerService> doorAction);
	IPhysicalDoor CreatePhysicalDoor(Action<IPhysicalDoor, IStreamerService> doorAction);
	IDoor? FindOne(Predicate<IDoor> filter);
	IEnumerable<IDoor> FindMany(Predicate<IDoor> filter);
	bool DestroyDoor(IDoor door);
	bool DestroyDoor(long id);
}

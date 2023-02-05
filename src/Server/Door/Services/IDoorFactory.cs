using SampSharp.Entities.SAMP;
using Server.Door.Components;
using Server.Door.Entities;
using Server.SpatialGrid.Entities;

namespace Server.Door.Services;

public interface IDoorFactory
{
	IGrid Grid { get; }
	IEnumerable<IDoor> Doors { get; }
	ILogicalDoor CreateLogicalDoor(long id);
	IPhysicalDoor CreatePhysicalDoor(long id);
	IDoorInteraction CreateDoorInteraction(IDoor door, Vector3 position, int world, int interior);
	IDoor? FindOne(Predicate<IDoor> filter);
	IEnumerable<IDoor> FindMany(Predicate<IDoor> filter);
	bool DestroyDoor(IDoor door);
	bool DestroyDoor(long id);
}

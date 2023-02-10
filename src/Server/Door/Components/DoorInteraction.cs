using Server.Door.Entities;
using Server.SpatialGrid.Components;

namespace Server.Door.Components;

public sealed class DoorInteraction : BaseSpatialComponent, IDoorInteraction
{
	public IDoor Door { get; }
	public float Angle { get; set; }
	public int World { get; set; }
	public int Interior { get; set; }

	public DoorInteraction(IDoor door, float x, float y, float z, float angle, int world, int interior) : base(x, y, z, 2)
	{
		Door = door;
		Angle = angle;
		World = world;
		Interior = interior;
	}
}

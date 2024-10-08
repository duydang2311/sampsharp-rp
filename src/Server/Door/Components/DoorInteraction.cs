using Server.Door.Entities;
using Server.Geometry.Entities;
using Server.SpatialGrid.Components;

namespace Server.Door.Components;

public sealed class DoorInteraction : BaseSpatialComponent, IDoorInteraction
{
	public IDoor Door { get; }
	public float Z { get; set; }
	public float Angle { get; set; }
	public int World { get; set; }
	public int Interior { get; set; }

	public DoorInteraction(IDoor door, IArea area, float z, float angle, int world, int interior) : base(area)
	{
		Z = z;
		Door = door;
		Angle = angle;
		World = world;
		Interior = interior;
	}
}

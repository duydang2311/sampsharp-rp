using SampSharp.Streamer.Entities;

namespace Server.Door.Entities;

public class PhysicalDoor : BaseDoor, IPhysicalDoor
{
	public DynamicObject? Object { get; set; }

	public PhysicalDoor(float x, float y) : base(x, y) { }
}

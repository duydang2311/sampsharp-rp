using SampSharp.Streamer.Entities;
using Server.Door.Components;

namespace Server.Door.Entities;

public class PhysicalDoor : BaseDoor, IPhysicalDoor
{
	public DynamicObject? Object { get; set; }
	public IDoorInteraction? ObjectInteraction { get; set; }

	public PhysicalDoor(long id) : base(id) { }
}

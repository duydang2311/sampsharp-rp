using SampSharp.Streamer.Entities;
using Server.Door.Components;

namespace Server.Door.Entities;

public interface IPhysicalDoor : IDoor
{
	DynamicObject? Object { get; set; }
	IDoorInteraction? ObjectInteraction { get; set; }
}

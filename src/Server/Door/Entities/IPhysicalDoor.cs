using SampSharp.Streamer.Entities;

namespace Server.Door.Entities;

public interface IPhysicalDoor : IDoor
{
	DynamicObject? Object { get; set; }
}

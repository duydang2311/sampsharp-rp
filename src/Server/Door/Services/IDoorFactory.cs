using SampSharp.Streamer.Entities;
using Server.Door.Entities;

namespace Server.Door.Services;

public interface IDoorFactory
{
	ILogicalDoor CreateLogicalDoor(Action<ILogicalDoor, IStreamerService> doorAction);
	IPhysicalDoor CreatePhysicalDoor(Action<IPhysicalDoor, IStreamerService> doorAction);
}

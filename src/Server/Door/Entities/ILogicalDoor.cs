using SampSharp.Streamer.Entities;

namespace Server.Door.Entities;

public interface ILogicalDoor : IDoor
{
	DynamicCheckpoint? EntranceCheckpoint { get; set; }
	DynamicCheckpoint? ExitCheckpoint { get; set; }
}

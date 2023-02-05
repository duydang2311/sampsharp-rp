using SampSharp.Streamer.Entities;

namespace Server.Door.Entities;

public class LogicalDoor : BaseDoor, ILogicalDoor
{
	public DynamicCheckpoint? EntranceCheckpoint { get; set; }
	public DynamicCheckpoint? ExitCheckpoint { get; set; }

	public LogicalDoor(float x, float y) : base(x, y) { }
}

using Server.Door.Components;

namespace Server.Door.Entities;

public class LogicalDoor : BaseDoor, ILogicalDoor
{
	public IDoorInteraction? EntranceInteraction { get; set; }
	public IDoorInteraction? ExitInteraction { get; set; }

	public LogicalDoor(long id) : base(id) { }
}

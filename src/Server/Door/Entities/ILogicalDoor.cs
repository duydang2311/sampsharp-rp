using Server.Door.Components;

namespace Server.Door.Entities;

public interface ILogicalDoor : IDoor
{
	IDoorInteraction? EntranceInteraction { get; set; }
	IDoorInteraction? ExitInteraction { get; set; }
}

using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Common.Colors;
using Server.Door.Entities;

namespace Server.Door.Systems.DoorCommand;

public sealed partial class DoorCommandSystem : ISystem
{
	private void HelpUpdateDoorEntrance(Player player)
	{
		chatService.SendMessage(player, b => b
			.AddBadge(m => m.Badge_CommandUsage)
			.Inline(m => m.DoorCommand_Entrance_Help));
	}

	private void UpdateDoorEntrance(Player player, string? argument)
	{
		if (!long.TryParse(argument, out var id))
		{
			HelpUpdateDoorEntrance(player);
			return;
		}

		if (!doorFactory.TryGetDoor(id, out var door))
		{
			chatService.SendMessage(player, b => b.Add(SemanticColor.Error, m => m.DoorCommand_Entrance_InvalidId, id));
			return;
		}

		if (door is not ILogicalDoor logicalDoor)
		{
			chatService.SendMessage(player, b => b.Add(SemanticColor.Error, m => m.DoorCommand_Entrance_InvalidDoorType, id));
			return;
		}

		if (logicalDoor.EntranceInteraction is not null)
		{
			doorFactory.Grid.Remove(logicalDoor.EntranceInteraction);
		}
		logicalDoor.EntranceInteraction = doorFactory.CreateDoorInteraction(logicalDoor, player.Position, player.VirtualWorld, player.Interior);
		chatService.SendMessage(player, b => b.Add(SemanticColor.Success, m => m.DoorCommand_Entrance_Success, id));
	}
}

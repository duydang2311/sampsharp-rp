using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Common.Colors;
using Server.Door.Entities;

namespace Server.Door.Systems.DoorCommand;

public sealed partial class DoorCommandSystem : ISystem
{
	private void HelpUpdateDoorExit(Player player)
	{
		chatService.SendMessage(player, b => b
			.AddBadge(t => t.Badge_CommandUsage)
			.Inline(t => t.DoorCommand_Exit_Help));
	}

	private void UpdateDoorExit(Player player, string? argument)
	{
		if (!long.TryParse(argument, out var id))
		{
			HelpUpdateDoorExit(player);
			return;
		}

		if (!doorFactory.TryGetDoor(id, out var door))
		{
			chatService.SendMessage(player, b => b.Add(SemanticColor.Error, m => m.DoorCommand_Exit_InvalidId, id));
			return;
		}

		if (door is not ILogicalDoor logicalDoor)
		{
			chatService.SendMessage(player, b => b.Add(SemanticColor.Error, m => m.DoorCommand_Exit_InvalidDoorType, id));
			return;
		}

		if (logicalDoor.ExitInteraction is not null)
		{
			doorFactory.Grid.Remove(logicalDoor.ExitInteraction);
		}
		logicalDoor.ExitInteraction = doorFactory.CreateDoorInteraction(logicalDoor, player.Position, player.VirtualWorld, player.Interior);
		chatService.SendMessage(player, b => b.Add(SemanticColor.Success, m => m.DoorCommand_Exit_Success, id));
	}
}

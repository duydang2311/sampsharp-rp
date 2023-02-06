using Microsoft.EntityFrameworkCore;
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
			.Add(t => t.Badge_Help)
			.Inline(t => t.DoorCommand_Entrance_Help));
	}

	private async Task UpdateDoorEntrance(Player player, string? argument)
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
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Error)
				.Inline(t => t.DoorCommand_Entrance_InvalidDoorType, id));
			return;
		}

		var position = player.Position;
		var world = player.VirtualWorld;
		var interior = player.Interior;
		if (logicalDoor.EntranceInteraction is not null)
		{
			doorFactory.Grid.Remove(logicalDoor.EntranceInteraction);
		}
		logicalDoor.EntranceInteraction = doorFactory.CreateDoorInteraction(logicalDoor, position, world, interior);
		await using var ctx = await dbContextFactory.CreateDbContextAsync();
		await ctx
			.Doors
			.Where(m => m.Id == id)
			.ExecuteUpdateAsync(m => m
				.SetProperty(m => m.EntranceX, position.X)
				.SetProperty(m => m.EntranceY, position.Y)
				.SetProperty(m => m.EntranceZ, position.Z)
				.SetProperty(m => m.EntranceWorld, world)
				.SetProperty(m => m.EntranceInterior, interior));
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Success)
			.Inline(t => t.DoorCommand_Entrance_Success, id));
	}
}

using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Common.Colors;
using Server.Door.Models;

namespace Server.Door.Systems.DoorCommand;

public sealed partial class DoorCommandSystem : ISystem
{
	private void HelpCreateDoor(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Help)
			.Inline(t => t.DoorCommand_Create_Help)
			.Add(SemanticColor.LowAttention, t => t.DoorCommand_Create_Options));
	}

	private async Task CreateDoor(Player player, string? argument)
	{
		if (string.IsNullOrEmpty(argument))
		{
			HelpCreateDoor(player);
			return;
		}
		switch (argument.ToLowerInvariant())
		{
			case "logical":
				{
					var position = player.Position;
					var angle = player.Angle;
					var world = player.VirtualWorld;
					var interior = player.Interior;

					await using var ctx = await dbContextFactory.CreateDbContextAsync();
					var model = new DoorModel()
					{
						EntranceX = position.X,
						EntranceY = position.Y,
						EntranceZ = position.Z,
						EntranceA = angle,
						EntranceWorld = world,
						EntranceInterior = interior,
					};
					ctx.Doors.Add(model);
					await ctx.SaveChangesAsync();
					if (model.Id == 0)
					{
						chatService.SendMessage(player, b => b
							.Add(t => t.Badge_Warning)
							.Inline(SemanticColor.LowAttention, t => t.DoorCommand_Create_NoEffect));
						return;
					}
					var door = doorFactory.CreateLogicalDoor(model.Id);
					door.EntranceInteraction = doorFactory.CreateDoorInteraction(door, position, world, interior);
					chatService.SendMessage(player, b => b
						.Add(t => t.Badge_Success)
						.Inline(t => t.DoorCommand_Create_Success, model.Id));
					break;
				}
			case "physical":
				{
					break;
				}
			default:
				{
					HelpCreateDoor(player);
					break;
				}
		}
	}
}

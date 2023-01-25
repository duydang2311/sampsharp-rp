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
			.Add(SemanticColor.Info, m => m.DoorCommand_Create_Help)
			.Add(SemanticColor.Info, m => m.DoorCommand_Create_Options));
	}

	private async Task CreateDoor(Player player, string argument)
	{
		switch (argument.ToLowerInvariant())
		{
			case "logical":
				{
					await using var ctx = await dbContextFactory.CreateDbContextAsync();
					var model = new DoorModel()
					{
						EntranceX = player.Position.X,
						EntranceY = player.Position.Y,
						EntranceZ = player.Position.Z,
						EntranceA = player.Angle,
						EntranceWorld = player.VirtualWorld,
						EntranceInterior = player.Interior,
					};
					_ = await ctx.Doors.AddAsync(model);
					if (model.Id == 0)
					{
						chatService.SendMessage(player, b => b
							.Add(SemanticColor.Info, m => m.DoorCommand_Create_NoEffect));
						return;
					}
					chatService.SendMessage(player, b => b
						.Add(SemanticColor.Success, m => m.DoorCommand_Create_Success, model.Id));
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

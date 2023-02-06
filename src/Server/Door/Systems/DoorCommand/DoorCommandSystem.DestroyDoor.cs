using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Common.Colors;

namespace Server.Door.Systems.DoorCommand;

public sealed partial class DoorCommandSystem : ISystem
{
	private void HelpDestroyDoor(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Help)
			.Inline(t => t.DoorCommand_Destroy_Help));
	}

	private async Task DestroyDoor(Player player, string? argument)
	{
		if (!long.TryParse(argument, out var id))
		{
			HelpDestroyDoor(player);
			return;
		}

		var ok = doorFactory.DestroyDoor(id);
		if (!ok)
		{
			chatService.SendMessage(player, b => b
				.Add(Color.Gray, m => m.DoorCommand_Destroy_InvalidId, id));
			return;
		}

		await using var ctx = await dbContextFactory.CreateDbContextAsync();
		var affected = await ctx.Doors.Where(m => m.Id == id).ExecuteDeleteAsync();
		if (affected == 0)
		{
			chatService.SendMessage(player, b => b
				.Inline(SemanticColor.Success, m => m.DoorCommand_Destroy_SuccessNoEffect, id));
		}
		else
		{
			chatService.SendMessage(player, b => b
				.Inline(SemanticColor.Success, m => m.DoorCommand_Destroy_Success, id));
		}
	}
}

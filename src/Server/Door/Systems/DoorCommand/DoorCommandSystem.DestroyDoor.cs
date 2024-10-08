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
				.Add(t => t.Badge_Warning)
				.Inline(t => t.DoorCommand_Destroy_InvalidId, id));
			return;
		}

		using var ctx = dbContextFactory.CreateDbContext();
		var affected = await ctx.Doors.Where(m => m.Id == id).ExecuteDeleteAsync().ConfigureAwait(false);
		if (affected == 0)
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Success)
				.Inline(t => t.DoorCommand_Destroy_SuccessNoEffect, id));
		}
		else
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Success)
				.Inline(t => t.DoorCommand_Destroy_Success, id));
		}
	}
}

using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Common.Colors;

namespace Server.Door.Systems.DoorCommand;

public sealed partial class DoorCommandSystem : ISystem
{
	private async Task NearbyDoor(Player player, string? argument)
	{
		var distanceSquared = 15f * 15f;
		if (!float.TryParse(argument, out var dist))
		{
			distanceSquared = dist * dist;
		}

		var x = player.Position.X;
		var y = player.Position.Y;
		var z = player.Position.Z;
		var world = player.VirtualWorld;
		var interior = player.Interior;
		await using var ctx = await dbContextFactory.CreateDbContextAsync();
		var models = await ctx.Doors
			.Where(m =>
				world == m.EntranceWorld &&
				interior == m.EntranceInterior &&
				Math.Pow(x - m.EntranceX, 2) +
				Math.Pow(y - m.EntranceY, 2) +
				Math.Pow(z - m.EntranceZ, 2) <= distanceSquared)
			.Select(m => new { m.Id, m.EntranceX, m.EntranceY, m.EntranceZ })
			.AsNoTracking()
			.ToArrayAsync();
		if (models.Length == 0)
		{
			chatService.SendMessage(player, b => b.Add(SemanticColor.Neutral, m => m.DoorCommand_Nearby_Empty));
			return;
		}

		chatService.SendMessage(player, b =>
		{
			b.Add(SemanticColor.Neutral, m => m.DoorCommand_Nearby_Found);
			foreach (var model in models)
			{
				b.Add(SemanticColor.Neutral, m => m.DoorCommand_Nearby_ForEachInfo, model.Id, model.EntranceX,
					model.EntranceY, model.EntranceZ,
					Math.Sqrt(Math.Pow(x - model.EntranceX, 2) + Math.Pow(x - model.EntranceY, 2) +
							  Math.Pow(z - model.EntranceZ, 2)));
			}
		});
	}
}

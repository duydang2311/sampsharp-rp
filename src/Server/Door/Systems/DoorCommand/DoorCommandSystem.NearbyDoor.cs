using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Common.Colors;

namespace Server.Door.Systems.DoorCommand;

public sealed partial class DoorCommandSystem : ISystem
{
	private async Task NearbyDoor(Player player, string? argument)
	{
		if (string.IsNullOrEmpty(argument) || !float.TryParse(argument, out var dist))
		{
			dist = 15f;
		}

		var x = player.Position.X;
		var y = player.Position.Y;
		var z = player.Position.Z;
		var world = player.VirtualWorld;
		var interior = player.Interior;
		using var ctx = dbContextFactory.CreateDbContext();
		var models = await ctx.Doors
			.Where(m =>
				world == m.EntranceWorld &&
				interior == m.EntranceInterior &&
				(Math.Pow(x - m.EntranceX, 2) + Math.Pow(y - m.EntranceY, 2) + Math.Pow(z - m.EntranceZ, 2)) <= dist * dist)
			.Select(m => new
			{
				m.Id,
				m.EntranceX,
				m.EntranceY,
				m.EntranceZ,
				m.EntranceWorld,
				m.EntranceInterior,
			})
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);
		if (models.Length == 0)
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Success)
				.Inline(t => t.DoorCommand_Nearby_Empty, dist));
			return;
		}
		Array.Sort(models, (a, b) =>
		{
			var distanceSquaredA = Math.Pow(x - a.EntranceX, 2) + Math.Pow(y - a.EntranceY, 2) + Math.Pow(z - a.EntranceZ, 2);
			var distanceSquaredB = Math.Pow(x - b.EntranceX, 2) + Math.Pow(y - b.EntranceY, 2) + Math.Pow(z - b.EntranceZ, 2);
			return (int)Math.Ceiling(distanceSquaredA - distanceSquaredB);
		});

		chatService.SendMessage(player, b =>
		{
			var count = 1;
			b
				.Add(t => t.Badge_Success)
				.Inline(t => t.DoorCommand_Nearby_Found, dist);
			foreach (var model in models)
			{
				b
					.Add(SemanticColor.LowAttention, $"{count++}.")
					.Inline(m => m.DoorCommand_Nearby_ForEachInfo, model.Id, model.EntranceX,
					model.EntranceY, model.EntranceZ,
					Math.Sqrt(Math.Pow(x - model.EntranceX, 2) + Math.Pow(y - model.EntranceY, 2) +
							  Math.Pow(z - model.EntranceZ, 2)));
			}
		});
	}
}

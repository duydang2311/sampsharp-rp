using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using SampSharp.Streamer.Entities;

namespace Server.SAMP.Streamer.Systems.ItemUpdate;

public sealed class ToggleItemUpdateSystem : ISystem
{
	[Event]
	private void OnPlayerConnect(Player player, IStreamerService streamerService)
	{
		streamerService.ToggleItemUpdate(player, StreamerType.Object, false);
		streamerService.ToggleItemUpdate(player, StreamerType.Pickup, false);
		streamerService.ToggleItemUpdate(player, StreamerType.Checkpoint, false);
		streamerService.ToggleItemUpdate(player, StreamerType.RaceCheckpoint, false);
		streamerService.ToggleItemUpdate(player, StreamerType.MapIcon, false);
		streamerService.ToggleItemUpdate(player, StreamerType.TextLabel, false);
		streamerService.ToggleItemUpdate(player, StreamerType.Area, false);
		streamerService.ToggleItemUpdate(player, StreamerType.Actor, false);
	}
}

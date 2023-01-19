using SampSharp.Entities;

namespace Server.Character.Systems.RolePlayCommands;
using SampSharp.Entities.SAMP;
using Server.Chat.Components;
public sealed class ConnectPlayerSystem : ISystem{

	[Event]
	private void OnPlayerConnect(Player player)
	{
		player.AddComponent<PermissionComponent>(PermissionLevel.Player);
	}
}
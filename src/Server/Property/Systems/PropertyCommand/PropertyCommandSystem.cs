using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using SampSharp.Entities.SAMP.Commands;

namespace Server.Property.Systems.PropertyCommand;

public sealed class PropertyCommandSystem : ISystem
{
	public PropertyCommandSystem() { }

	private void Help(Player player)
	{
		player.SendClientMessage(Color.White, "Hướng dẫn: /property [option].");
	}

	[PlayerCommand("property")]
	private void PropertyCommand(Player player, string option, int? a)
	{
		switch (option)
		{
			case "create":
				{
					break;
				}
			default:
				{
					Help(player);
					break;
				}
		}
	}
}

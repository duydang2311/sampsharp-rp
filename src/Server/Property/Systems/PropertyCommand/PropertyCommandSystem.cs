using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using SampSharp.Entities.SAMP.Commands;
using Server.Chat.Components;
using Server.Chat.Services;
using Server.Common.Colors;

namespace Server.Property.Systems.PropertyCommand;

public sealed class PropertyCommandSystem : ISystem
{
	private readonly IChatService chatService;

	public PropertyCommandSystem(ICommandService commandService, IChatService chatService)
	{
		this.chatService = chatService;
		commandService.RegisterCommand(m =>
		{
			m.Name = "property";
			m.Delegate = PropertyCommand;
			m.PermissionLevel = PermissionLevel.Admin;
		});
	}

	private void HelpPropertyCommand(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Help)
			.Inline(t => t.PropertyCommand_Help)
			.Add(SemanticColor.LowAttention, t => t.PropertyCommand_Options));
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
					HelpPropertyCommand(player);
					break;
				}
		}
	}
}

using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;
using Server.Common.Colors;
using Server.I18N.Localization.Services;

namespace Server.Character.Systems.RolePlayCommands;

public sealed class DoCommandSystem : ISystem
{
	private readonly IChatService chatService;

	public DoCommandSystem(ICommandService commandService, IChatService chatService)
	{
		this.chatService = chatService;
		commandService.RegisterCommand((c) =>
		{
			c.Name = "do";
			c.Delegate = DoCommand;
		});
		commandService.RegisterHelper("do", DoCommandHelp);
	}

	public void DoCommand(Player player, string text)
	{
		
	}

	public void DoCommandHelp(Player player)
	{
		
	}
}
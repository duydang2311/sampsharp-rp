using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;
using Server.Common.Colors;

namespace Server.Character.Systems.RolePlayCommands;

public sealed class DoCommandSystem : ISystem
{
	private readonly IChatService chatService;

	public DoCommandSystem(ICommandService commandService, IChatService chatService)
	{
		this.chatService = chatService;
		commandService.RegisterCommand(m =>
		{
			m.Name = "do";
			m.Delegate = DoCommand;
			m.HelpDelegate = DoCommandHelp;
		});
	}

	public void DoCommand(Player player, string text)
	{
		chatService.SendMessage(
			p => Vector3.DistanceSquared(p.Position, player.Position) <= 15f * 15f,
			b => b.Add(SemanticColor.Roleplay, ex => ex.DoCommandText, text, player.Name));
	}

	public void DoCommandHelp(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(model => model.Badge_Help)
			.Inline(model => model.DoCommandHelp));
	}
}

using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;
using Server.Common.Colors;

namespace Server.Character.Systems.RolePlayCommands;

public sealed class LowCommandSystem : ISystem
{
	private readonly IChatService chatService;

	public LowCommandSystem(ICommandService commandService, IChatService chatService)
	{
		this.chatService = chatService;
		commandService.RegisterCommand(m =>
		{
			m.Name = "low";
			m.Delegate = LowCommand;
			m.HelpDelegate = HelpLowCommand;
		});
		commandService.RegisterAlias("low", "l");

	}
	public void LowCommand(Player player, string text)
	{
		chatService.SendMessage(
			p => Vector3.DistanceSquared(p.Position, player.Position) <= 7f * 7f,
			b => b.Add(SemanticColor.LowAttention, model => model.LowCommand_Text, player.Name, text)
		);
	}
	public void HelpLowCommand(Player player)
	{
		chatService.SendMessage(
			player,
			b => b
				.Add(model => model.Badge_Help)
				.Inline(SemanticColor.LowAttention, model => model.LowCommand_Help)
		);
	}
}


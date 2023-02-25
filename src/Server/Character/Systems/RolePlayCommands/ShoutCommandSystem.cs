using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;
using Server.Common.Colors;

namespace Server.Character.Systems.RolePlayCommands;

public sealed class ShoutCommandSystem : ISystem
{
	private readonly IChatService chatService;

	public ShoutCommandSystem(ICommandService commandService, IChatService chatService)
	{
		this.chatService = chatService;
		commandService.RegisterCommand(m =>
		{
			m.Name = "shout";
			m.Delegate = ShoutCommand;
			m.HelpDelegate = HelpShoutCommand;
		});
		commandService.RegisterAlias("shout", "s");
	}
	public void ShoutCommand(Player player, string text)
	{
		chatService.SendMessage(
			p => Vector3.DistanceSquared(p.Position, player.Position) <= 15f * 15f,
			b => b.Add(SemanticColor.Shout, model => model.ShoutCommand_Text, player.Name, text)
		);
	}
	public void HelpShoutCommand(Player player)
	{
		chatService.SendMessage(
			player,
			b => b
				.Add(model => model.Badge_Help)
				.Inline(SemanticColor.Shout, model => model.ShoutCommand_Help)
		);
	}
}

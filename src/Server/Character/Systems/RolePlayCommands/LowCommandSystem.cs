

using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;
using Server.Common.Colors;
using Server.I18N.Localization.Services;

public sealed class LowCommandSystem : ISystem
{
	private readonly IChatService chatService;
	private readonly ITextLocalizerService textLocalizerService;

	public LowCommandSystem(ICommandService commandService, IChatService chatService,
		ITextLocalizerService textLocalizerService)
	{
		this.chatService = chatService;
		this.textLocalizerService = textLocalizerService;
		commandService.RegisterCommand(m =>
		{
			m.Name = "l";
			m.Delegate = LowCommand;
		});
		commandService.RegisterCommand(m =>
		{
			m.Name = "low";
			m.Delegate = LowCommand;
		});
		commandService.RegisterHelper("l", HelpLowCommand);
		commandService.RegisterHelper("low", HelpLowCommand);

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
				.Inline(model => model.LowCommand_Help)
		);
	}
}


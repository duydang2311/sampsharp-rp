

using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;
using Server.Common.Colors;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

public sealed class AmeCommandSystem : ISystem
{

	private readonly ITextLocalizerService textLocalizerService;
	private readonly IChatService chatService;

	public AmeCommandSystem(ICommandService commandService, IChatService chatService,
		ITextLocalizerService textLocalizerService)
	{
		this.textLocalizerService = textLocalizerService;
		this.chatService = chatService;
		commandService.RegisterCommand(m =>
		{
			m.Name = "ame";
			m.Delegate = AmeCommand;
		});
		commandService.RegisterHelper("ame", HelpAmeCommand);
	}
	public void AmeCommand(Player player, string input)
	{
		player.SetChatBubble(textLocalizerService.Get(nameof(ILocalizedText.AmeCommand_Text), input), SemanticColor.Roleplay, 15f, 5000);
	}
	public void HelpAmeCommand(Player player)
	{
		chatService.SendMessage(
			player,
			b => b
				.Add(m => m.Badge_Help)
				.Inline(m => m.AmeCommand_Help)
		);
	}
}
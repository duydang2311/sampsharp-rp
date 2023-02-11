using System.Globalization;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Components;
using Server.Chat.Services;
using Server.Common.Colors;
using Server.I18N.Localization.Components;

namespace Server.I18N.Localization.Systems.LanguageCommand;

public sealed class LanguageCommandSystem : ISystem
{
	private readonly IChatService chatService;
	public LanguageCommandSystem(ICommandService commandService, IChatService chatService)
	{
		this.chatService = chatService;
		commandService.RegisterCommand(m =>
		{
			m.Name = "language";
			m.PermissionLevel = PermissionLevel.None;
			m.Delegate = ChangeLanguage;
		});
		commandService.RegisterHelper("language", HelpChangeLanguage);
	}

	private void HelpChangeLanguage(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Help)
			.Inline(t => t.LanguageCommand_Help)
			.Add(SemanticColor.LowAttention, t => t.LanguageCommand_Options));
	}

	private void ChangeLanguage(Player player, string locale)
	{
		if (locale != "vi" && locale != "vi-VN" && locale != "en")
		{
			HelpChangeLanguage(player);
			return;
		}

		player.GetComponent<CultureComponent>().Culture = CultureInfo.GetCultureInfo(locale);
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Success)
			.Inline(t => t.LanguageCommand_Success, locale));
	}
}

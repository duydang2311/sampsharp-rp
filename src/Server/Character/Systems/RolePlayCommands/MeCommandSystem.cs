using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Components;
using Server.Chat.Models;
using Server.Chat.Services;

using Server.Common.Colors;

using Server.I18N.Localization.Services;

namespace Server.Character.Systems.RolePlayCommands;

public sealed class MeCommandSystem : ISystem
{
	private readonly IChatService chatService;
	private readonly ITextLocalizerService textLocalizerService;

	public MeCommandSystem(ICommandService commandService, IChatService chatService,
		ITextLocalizerService textLocalizerService)
	{
		this.chatService = chatService;
		this.textLocalizerService = textLocalizerService;
		commandService.RegisterCommand((model) =>
		{
			model.Name = "me";
			model.Delegate = MeCommand;
		});
		commandService.RegisterHelper("me", HelpMeCommand);
	}

	public void MeCommand(Player player, string text)
	{
		chatService.SendMessage(

			p  => (Vector3.Distance(p.Position, player.Position) < 15f * 15f),
			a => a.Add(SemanticColor.Roleplay, model => model.MeCommandText, player.Name, text)
		);

	}


	public void HelpMeCommand(Player player)
	{

	}
}
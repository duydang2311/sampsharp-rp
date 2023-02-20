

using SampSharp.Entities;
using Server.Chat.Services;
using Server.I18N.Localization.Services;
using Server.Common.Colors;
using SampSharp.Entities.SAMP;

public sealed class ShoutCommandSystem : ISystem
{
	private readonly IChatService chatService;
	private readonly ITextLocalizerService textLocalizerService;

	public ShoutCommandSystem(ICommandService commandService, IChatService chatService,
		ITextLocalizerService textLocalizerService)
	{
		this.chatService = chatService;
		this.textLocalizerService = textLocalizerService;
		commandService.RegisterCommand(modelAction =>
		{
			modelAction.Name = "s";
			modelAction.Delegate = ShoutCommand;
		});
		commandService.RegisterCommand(modelAction =>
		{
			modelAction.Name = "shout";
			modelAction.Delegate = ShoutCommand;
		});
		commandService.RegisterHelper("s", HelpShoutCommand);
		commandService.RegisterHelper("shout", HelpShoutCommand);
	}
	public void ShoutCommand(Player player, string text)
	{
		chatService.SendMessage(
			p => (Vector3.DistanceSquared(p.Position, player.Position) <= 15f * 15f),
			b => b.Add(SemanticColor.Shout, model => model.ShoutCommand_Text, player.Name, text)
		);
	}
	public void HelpShoutCommand(Player player)
	{
        chatService.SendMessage(
            player, 
            b => b
                .Add(model => model.Badge_Help) 
                .Inline(model => model.ShoutCommand_Help)
        );
	}

}
using Server.I18N.Localization.Services;

namespace Server.Chat.Services;

public sealed class ChatMessageBuilderFactory : IChatMessageBuilderFactory
{
	private readonly ITextLocalizerService localizerService;
	private readonly ITextNameIdentifierService identifierService;

	public ChatMessageBuilderFactory(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService)
	{
		this.localizerService = localizerService;
		this.identifierService = identifierService;
	}

	public IChatMessageBuilder CreateBuilder()
	{
		return new ChatMessageBuilder(localizerService, identifierService);
	}
}

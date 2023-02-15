using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public sealed class DialogTextBuilderFactory : IDialogTextBuilderFactory
{
	private readonly ITextLocalizerService localizerService;
	private readonly ITextNameIdentifierService identifierService;

	public DialogTextBuilderFactory(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService)
	{
		this.localizerService = localizerService;
		this.identifierService = identifierService;
	}

	public IDialogTextBuilder CreateBuilder()
	{
		return new DialogTextBuilder(localizerService, identifierService);
	}
}

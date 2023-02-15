using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public sealed class DialogBuilderFactory : IDialogBuilderFactory
{
	private readonly ITextLocalizerService localizerService;
	private readonly ITextNameIdentifierService identifierService;
	private readonly ILocalizedTextBuilderFactory textBuilderFactory;

	public DialogBuilderFactory(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService, ILocalizedTextBuilderFactory textBuilderFactory)
	{
		this.localizerService = localizerService;
		this.identifierService = identifierService;
		this.textBuilderFactory = textBuilderFactory;
	}

	public IInputDialogBuilder CreateInputBuilder()
	{
		return new InputDialogBuilder(localizerService, identifierService);
	}

	public IListDialogBuilder CreateListBuilder()
	{
		return new ListDialogBuilder(localizerService, identifierService);
	}

	public IMessageDialogBuilder CreateMessageBuilder()
	{
		return new MessageDialogBuilder(localizerService, identifierService);
	}

	public ITablistDialogBuilder CreateTablistBuilder()
	{
		return new TablistDialogBuilder(localizerService, identifierService, textBuilderFactory);
	}
}

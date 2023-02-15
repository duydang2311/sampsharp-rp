namespace Server.I18N.Localization.Services;

public sealed class LocalizedTextBuilderFactory : ILocalizedTextBuilderFactory
{
	private readonly ITextLocalizerService localizerService;
	private readonly ITextNameIdentifierService identifierService;

	public LocalizedTextBuilderFactory(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService)
	{
		this.localizerService = localizerService;
		this.identifierService = identifierService;
	}
	public ILocalizedTextBuilder CreateBuilder()
	{
		return new LocalizedTextBuilder(localizerService, identifierService);
	}
}

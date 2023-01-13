using Server.I18n.Globalization.Services;
using Server.I18n.Localization.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ExtendIServiceCollection
{
	public static IServiceCollection WithI18N(this IServiceCollection self, Action<IGlobalCultureServiceOptions, ITextLocalizerServiceOptions> configI18N)
	{
		var globalCultureOptions = new GlobalCultureServiceOptions();
		var textLocalizerOptions = new TextLocalizerServiceOptions();
		configI18N(globalCultureOptions, textLocalizerOptions);
		return self
			.AddSingleton<IGlobalCultureServiceOptions>(_ => globalCultureOptions)
			.AddSingleton<ITextLocalizerServiceOptions>(_ => textLocalizerOptions)
			.AddSingleton<IGlobalCultureService, GlobalCultureService>()
			.AddSingleton<ITextLocalizerService, TextLocalizerService>()
			.AddSingleton<ITextNameIdentifierService, TextNameIdentifierService>();
	}
}

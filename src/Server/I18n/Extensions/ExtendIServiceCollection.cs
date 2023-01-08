using Server.I18n.Globalization.Services;
using Server.I18n.Localization.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ExtendIServiceCollection
{
	public static IServiceCollection WithI18n(this IServiceCollection self, Action<IGlobalCultureServiceOptions, ITextLocalizerServiceOptions> configI18n)
	{
		var globalCultureOptions = new GlobalCultureServiceOptions();
		var textLocalizerOptions = new TextLocalizerServiceOptions();
		configI18n(globalCultureOptions, textLocalizerOptions);
		return self
			.AddSingleton<IGlobalCultureServiceOptions>(provider => globalCultureOptions)
			.AddSingleton<ITextLocalizerServiceOptions>(provider => textLocalizerOptions)
			.AddSingleton<IGlobalCultureService, GlobalCultureService>()
			.AddSingleton<ITextLocalizerService, TextLocalizerService>()
			.AddSingleton<ITextNameIdentifierService, TextNameIdentifierService>();
	}
}

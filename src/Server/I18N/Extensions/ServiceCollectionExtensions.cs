using SampSharp.Entities;
using Server.I18N.Globalization.Services;
using Server.I18N.Localization.Services;
using Server.I18N.Localization.Systems.Connect;
using Server.I18N.Localization.Systems.LanguageCommand;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
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
			.AddSingleton<ITextNameIdentifierService, TextNameIdentifierService>()
			.AddSingleton<IPlayerTextLocalizerService, PlayerTextLocalizerService>()
			.AddSystem<ConnectSystem>()
			.AddSystem<LanguageCommandSystem>();
	}
}

using System.Globalization;
using System.Reflection;
using System.Resources;
using Server.I18n.Globalization.Services;

namespace Server.I18n.Localization.Services;

public sealed class TextLocalizerService : ITextLocalizerService
{
	private readonly Dictionary<CultureInfo, ResourceManager> resourceManagerDict = new();
	private readonly ITextLocalizerServiceOptions config;
	public TextLocalizerService(ITextLocalizerServiceOptions config, IGlobalCultureService globalCultureService)
	{
		this.config = config;
		foreach (var culture in globalCultureService.Cultures)
		{
			var ass = Assembly.GetExecutingAssembly().GetSatelliteAssembly(culture);
			resourceManagerDict.Add(culture, new ResourceManager($"{config.BaseName}.{culture.Name}", ass));
		}
	}

	public string Get(CultureInfo culture, string key)
	{
		return resourceManagerDict[culture].GetString(key) ?? string.Empty;
	}

	public string Get(CultureInfo culture, string key, params object[] args)
	{
		var text = resourceManagerDict[culture].GetString(key);
		if (text is null)
		{
			return string.Empty;
		}
		return string.Format(text, args);
	}

	public string Get(string key)
	{
		return Get(config.DefaultCulture, key);
	}

	public string Get(string key, params object[] args)
	{
		return Get(config.DefaultCulture, key, args);
	}
}

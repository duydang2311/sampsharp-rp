using System.Globalization;
using System.Reflection;
using System.Resources;
using Server.I18N.Globalization.Services;

namespace Server.I18N.Localization.Services;

public class TextLocalizerService : ITextLocalizerService
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
		return string.Format(Get(culture, key), args);
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

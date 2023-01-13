using System.Globalization;

namespace Server.I18N.Globalization.Services;

public sealed class GlobalCultureService : IGlobalCultureService
{
	public IEnumerable<CultureInfo> Cultures { get; }

	public GlobalCultureService(IGlobalCultureServiceOptions config)
	{
		Cultures = config.Cultures;
	}

	public bool HasCulture(string name)
	{
		return Cultures.Any(culture => culture.Name == name);
	}

	public bool HasCulture(CultureInfo culture)
	{
		return Cultures.Any(c => c.Equals(culture));
	}
}

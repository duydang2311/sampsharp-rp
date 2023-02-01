using System.Globalization;

namespace Server.I18N.Globalization.Services;

public sealed class GlobalCultureServiceOptions : IGlobalCultureServiceOptions
{
	private readonly LinkedList<CultureInfo> cultures = new();
	IEnumerable<CultureInfo> IGlobalCultureServiceOptions.Cultures => cultures;

	public void AddCulture(string name)
	{
		AddCulture(CultureInfo.GetCultureInfo(name));
	}

	public void AddCulture(CultureInfo culture)
	{
		cultures.AddLast(culture);
	}
}

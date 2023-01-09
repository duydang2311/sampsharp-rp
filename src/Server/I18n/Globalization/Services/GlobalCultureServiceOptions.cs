using System.Globalization;

namespace Server.I18n.Globalization.Services;

public sealed class GlobalCultureServiceOptions : IGlobalCultureServiceOptions
{
	public LinkedList<CultureInfo> cultures = new();
	IEnumerable<CultureInfo> IGlobalCultureServiceOptions.Cultures { get => cultures; }

	public void AddCulture(string name)
	{
		AddCulture(CultureInfo.GetCultureInfo(name));
	}

	public void AddCulture(CultureInfo culture)
	{
		cultures.AddLast(culture);
	}
}

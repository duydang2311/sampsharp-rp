using System.Globalization;

namespace Server.I18N.Globalization.Services;

public interface IGlobalCultureServiceOptions
{
	IEnumerable<CultureInfo> Cultures { get; }
	void AddCulture(string culture);
	void AddCulture(CultureInfo culture);
}

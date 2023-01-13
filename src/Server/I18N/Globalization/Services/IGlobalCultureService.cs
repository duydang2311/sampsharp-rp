using System.Globalization;

namespace Server.I18N.Globalization.Services;

public interface IGlobalCultureService
{
	IEnumerable<CultureInfo> Cultures { get; }
	bool HasCulture(string name);
	bool HasCulture(CultureInfo culture);
}

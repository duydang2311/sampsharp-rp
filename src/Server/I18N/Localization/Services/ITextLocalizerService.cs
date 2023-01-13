using System.Globalization;

namespace Server.I18N.Localization.Services;

public interface ITextLocalizerService
{
	string Get(CultureInfo culture, string key);
	string Get(CultureInfo culture, string key, params object[] args);
	string Get(string key);
	string Get(string key, params object[] args);
}

using System.Globalization;

namespace Server.I18n.Localization.Services;

public interface ITextLocalizerServiceOptions
{
	string BaseName { get; set; }
	CultureInfo DefaultCulture { get; set; }
}

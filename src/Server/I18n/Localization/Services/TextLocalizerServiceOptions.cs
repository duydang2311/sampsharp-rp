using System.Globalization;

namespace Server.I18n.Localization.Services;

public sealed class TextLocalizerServiceOptions : ITextLocalizerServiceOptions
{
	public string BaseName { get; set; } = string.Empty;
	public CultureInfo DefaultCulture { get; set; } = CultureInfo.CurrentCulture;
}

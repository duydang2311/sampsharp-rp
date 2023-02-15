namespace Server.I18N.Localization.Models;

public sealed class LocalizedTextModel
{
	public string Text { get; set; } = string.Empty;
	public object[] Args { get; init; } = Array.Empty<object>();
}

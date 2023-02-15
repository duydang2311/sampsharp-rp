namespace Server.I18N.Localization.Models;

public class LocalizedTextModel
{
	public string Text { get; set; } = string.Empty;
	public object[] Args { get; init; } = Array.Empty<object>();
	public bool IsLocal { get; set; } = false;
}

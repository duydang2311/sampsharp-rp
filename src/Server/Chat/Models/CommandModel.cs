using Server.Chat.Components;

namespace Server.Chat.Models;

public sealed class CommandModel
{
	public string Name { get; set; } = string.Empty;
	public PermissionLevel PermissionLevel { get; set; }
	public Delegate? Delegate { get; set; }
}

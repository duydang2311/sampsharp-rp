using SampSharp.Entities;

namespace Server.Chat.Components;

[Flags]
public enum PermissionLevel : int
{
	None = 0b_0000,
	Admin = 0b_0001
}

public sealed class PermissionComponent : Component
{
	public PermissionLevel Level { get; set; } = PermissionLevel.None;
}

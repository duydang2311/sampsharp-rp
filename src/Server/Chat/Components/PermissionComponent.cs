using SampSharp.Entities;

namespace Server.Chat.Components;

[Flags]
public enum PermissionLevel : int
{
	None = 0b_0000,
	Player = 0b_0001,
	Admin = 0b_0010
}

public sealed class PermissionComponent : Component
{
	public PermissionLevel Level { get; set; }
    public PermissionComponent(PermissionLevel level)
    {
        Level = level;
    }
}

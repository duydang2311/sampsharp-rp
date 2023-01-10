using Server.Chat.Components;
using Server.Chat.Models;

namespace Server.Chat.Services;

public interface ICommandModelFactory
{
	CommandModel Create();
	CommandModel Create(string name);
	CommandModel Create(string name, PermissionLevel permissionLevel);
}

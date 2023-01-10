using Server.Chat.Components;
using Server.Chat.Models;

namespace Server.Chat.Services;

public sealed class CommandModelFactory : ICommandModelFactory
{
	public CommandModel Create(string name)
	{
		return new CommandModel { Name = name };
	}
	public CommandModel Create(string name, PermissionLevel permissionLevel)
	{
		return new CommandModel { Name = name, PermissionLevel = permissionLevel };
	}
}

using Server.Chat.Models;

namespace Server.Chat.Services;

public interface ICommandService
{
	bool HasCommand(string command);
	bool HasHelper(string command);
	CommandModel GetCommandModel(string command);
	object? InvokeCommand(string command, object?[]? arguments);
	object? InvokeHelper(string command, object?[]? arguments);
	void RegisterCommand(Action<CommandModel> modelAction);
	void RegisterAlias(string command, params string[] aliases);
}

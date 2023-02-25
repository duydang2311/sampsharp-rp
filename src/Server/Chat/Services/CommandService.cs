using Server.Chat.Components;
using Server.Chat.Models;

namespace Server.Chat.Services;

public sealed class CommandService : ICommandService
{
	private readonly ICommandModelFactory commandModelFactory;
	private readonly IDictionary<string, CommandModel> commandDict = new Dictionary<string, CommandModel>();

	public CommandService(ICommandModelFactory commandModelFactory)
	{
		this.commandModelFactory = commandModelFactory;
	}

	public bool HasCommand(string command)
	{
		return commandDict.ContainsKey(command);
	}

	public bool HasHelper(string command)
	{
		return commandDict[command].HelpDelegate is not null;
	}

	public CommandModel GetCommandModel(string command)
	{
		return commandDict[command];
	}

	public object? InvokeCommand(string command, object?[]? arguments)
	{
		var @delegate = commandDict[command].Delegate;
		if (@delegate is null)
		{
			return default;
		}
		var method = @delegate.Method;
		return method.Invoke(@delegate.Target, arguments);
	}

	public object? InvokeHelper(string command, object?[]? arguments)
	{
		var @delegate = commandDict[command].HelpDelegate!;
		var method = @delegate.Method;
		return method.Invoke(@delegate.Target, arguments);
	}

	public void RegisterCommand(Action<CommandModel> modelAction)
	{
		var model = commandModelFactory.Create();
		model.PermissionLevel = PermissionLevel.Player;
		modelAction(model);
		commandDict.Add(model.Name, model);
	}

	public void RegisterAlias(string command, params string[] aliases)
	{
		var model = commandDict[command];
		foreach (var alias in aliases)
		{
			commandDict.Add(alias, model);
		}
	}
}

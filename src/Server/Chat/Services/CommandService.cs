using Server.Chat.Models;

namespace Server.Chat.Services;

public sealed class CommandService : ICommandService
{
	private readonly ICommandModelFactory commandModelFactory;
	private readonly IDictionary<string, CommandModel> commandDict = new Dictionary<string, CommandModel>();
	private readonly IDictionary<string, Delegate> helperDict = new Dictionary<string, Delegate>();

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
		return helperDict.ContainsKey(command);
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
		var @delegate = helperDict[command];
		var method = @delegate.Method;
		return method.Invoke(@delegate.Target, arguments);
	}

	public void RegisterHelper(string command, Delegate handler)
	{
		helperDict.Add(command, handler);
	}

	public void RegisterCommand(Action<CommandModel> modelAction)
	{
		var model = commandModelFactory.Create();
		modelAction(model);
		commandDict.Add(model.Name, model);
	}
}

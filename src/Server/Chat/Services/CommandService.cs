using SampSharp.Entities.SAMP;

namespace Server.Chat.Services;

public sealed class CommandService : ICommandService
{
	private readonly IDictionary<string, Delegate> commandDict = new Dictionary<string, Delegate>();
	private readonly IDictionary<string, Delegate> helperDict = new Dictionary<string, Delegate>();
	public bool HasCommand(string command)
	{
		return commandDict.ContainsKey(command);
	}

	public bool HasHelper(string command)
	{
		return helperDict.ContainsKey(command);
	}

	public object? InvokeCommand(string command, object?[]? arguments)
	{
		var @delegate = commandDict[command];
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

	public void RegisterCommand(string command, Delegate handler)
	{
		commandDict.Add(command, handler);
	}

	public void RegisterCommand(string command, Action<Player> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1>(string command, Action<Player, T1> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2>(string command, Action<Player, T1, T2> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2, T3>(string command, Action<Player, T1, T2, T3> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2, T3, T4>(string command, Action<Player, T1, T2, T3, T4> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2, T3, T4, T5>(string command, Action<Player, T1, T2, T3, T4, T5> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2, T3, T4, T5, T6>(string command, Action<Player, T1, T2, T3, T4, T5, T6> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2, T3, T4, T5, T6, T7>(string command, Action<Player, T1, T2, T3, T4, T5, T6, T7> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand(string command, Func<Player> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1>(string command, Func<Player, T1, Task> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2>(string command, Func<Player, T1, T2, Task> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2, T3>(string command, Func<Player, T1, T2, T3, Task> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2, T3, T4>(string command, Func<Player, T1, T2, T3, T4, Task> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2, T3, T4, T5>(string command, Func<Player, T1, T2, T3, T4, T5, Task> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2, T3, T4, T5, T6>(string command, Func<Player, T1, T2, T3, T4, T5, T6, Task> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}

	public void RegisterCommand<T1, T2, T3, T4, T5, T6, T7>(string command, Func<Player, T1, T2, T3, T4, T5, T6, T7, Task> handler)
	{
		RegisterCommand(command, handler as Delegate);
	}
}

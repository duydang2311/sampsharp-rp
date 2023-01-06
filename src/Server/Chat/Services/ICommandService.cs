using SampSharp.Entities.SAMP;

namespace Server.Chat.Services;

public interface ICommandService
{
	bool HasCommand(string command);
	bool HasHelper(string command);
	Delegate GetCommandDelegate(string command);
	object? InvokeCommand(string command, object?[]? arguments);
	object? InvokeHelper(string command, object?[]? arguments);
	void RegisterHelper(string command, Delegate handler);
	void RegisterCommand(string command, Delegate handler);
	void RegisterCommand(string command, Action<Player> handler);
	void RegisterCommand<T1>(string command, Action<Player, T1> handler);
	void RegisterCommand<T1, T2>(string command, Action<Player, T1, T2> handler);
	void RegisterCommand<T1, T2, T3>(string command, Action<Player, T1, T2, T3> handler);
	void RegisterCommand<T1, T2, T3, T4>(string command, Action<Player, T1, T2, T3, T4> handler);
	void RegisterCommand<T1, T2, T3, T4, T5>(string command, Action<Player, T1, T2, T3, T4, T5> handler);
	void RegisterCommand<T1, T2, T3, T4, T5, T6>(string command, Action<Player, T1, T2, T3, T4, T5, T6> handler);
	void RegisterCommand<T1, T2, T3, T4, T5, T6, T7>(string command, Action<Player, T1, T2, T3, T4, T5, T6, T7> handler);
	void RegisterCommand(string command, Func<Player> handler);
	void RegisterCommand<T1>(string command, Func<Player, T1, Task> handler);
	void RegisterCommand<T1, T2>(string command, Func<Player, T1, T2, Task> handler);
	void RegisterCommand<T1, T2, T3>(string command, Func<Player, T1, T2, T3, Task> handler);
	void RegisterCommand<T1, T2, T3, T4>(string command, Func<Player, T1, T2, T3, T4, Task> handler);
	void RegisterCommand<T1, T2, T3, T4, T5>(string command, Func<Player, T1, T2, T3, T4, T5, Task> handler);
	void RegisterCommand<T1, T2, T3, T4, T5, T6>(string command, Func<Player, T1, T2, T3, T4, T5, T6, Task> handler);
	void RegisterCommand<T1, T2, T3, T4, T5, T6, T7>(string command, Func<Player, T1, T2, T3, T4, T5, T6, T7, Task> handler);
}

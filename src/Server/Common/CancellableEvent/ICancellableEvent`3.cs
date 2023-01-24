using System.ComponentModel;

namespace Server.Common.CancellableEvent;

public interface ICancellableEvent<T1, T2, T3>
{
	void AddHandler(Action<T1, T2, T3, CancelEventArgs> handler);
	void AddHandler(Func<T1, T2, T3, CancelEventArgs, Task> handler);
	void RemoveHandler(Action<T1, T2, T3, CancelEventArgs> handler);
	void RemoveHandler(Func<T1, T2, T3, CancelEventArgs, Task> handler);
	void Invoke(T1 arg1, T2 arg2, T3 arg3);
	Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3);
}

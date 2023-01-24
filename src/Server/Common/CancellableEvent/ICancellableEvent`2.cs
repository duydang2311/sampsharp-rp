using System.ComponentModel;

namespace Server.Common.CancellableEvent;

public interface ICancellableEvent<T1, T2>
{
	void AddHandler(Action<T1, T2, CancelEventArgs> handler);
	void AddHandler(Func<T1, T2, CancelEventArgs, Task> handler);
	void RemoveHandler(Action<T1, T2, CancelEventArgs> handler);
	void RemoveHandler(Func<T1, T2, CancelEventArgs, Task> handler);
	void Invoke(T1 arg1, T2 arg2);
	Task InvokeAsync(T1 arg1, T2 arg2);
}

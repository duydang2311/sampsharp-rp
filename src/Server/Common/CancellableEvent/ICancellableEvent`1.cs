using System.ComponentModel;

namespace Server.Common.CancellableEvent;

public interface ICancellableEvent<T1>
{
	void AddHandler(Action<T1, CancelEventArgs> handler);
	void AddHandler(Func<T1, CancelEventArgs, Task> handler);
	void RemoveHandler(Action<T1, CancelEventArgs> handler);
	void RemoveHandler(Func<T1, CancelEventArgs, Task> handler);
	void Invoke(T1 arg1);
	Task InvokeAsync(T1 arg1);
}

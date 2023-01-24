using System.ComponentModel;

namespace Server.Common.CancellableEvent;

public interface ICancellableEvent
{
	void AddHandler(Action<CancelEventArgs> handler);
	void AddHandler(Func<CancelEventArgs, Task> handler);
	void RemoveHandler(Action<CancelEventArgs> handler);
	void RemoveHandler(Func<CancelEventArgs, Task> handler);
	void Invoke();
	Task InvokeAsync();
}

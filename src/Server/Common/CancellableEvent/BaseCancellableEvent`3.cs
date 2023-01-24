using System.ComponentModel;

namespace Server.Common.CancellableEvent;

public class BaseCancellableEvent<T1, T2, T3> : ICancellableEvent<T1, T2, T3>
{
	private readonly ICancellableEventInvoker invoker;

	protected event Action<T1, T2, T3, CancelEventArgs>? Event;
	protected event Func<T1, T2, T3, CancelEventArgs, Task>? AsyncEvent;

	public BaseCancellableEvent(ICancellableEventInvoker invoker)
	{
		this.invoker = invoker;
	}

	public void AddHandler(Action<T1, T2, T3, CancelEventArgs> handler)
	{
		Event += handler;
	}

	public void AddHandler(Func<T1, T2, T3, CancelEventArgs, Task> handler)
	{
		AsyncEvent += handler;
	}

	public void RemoveHandler(Action<T1, T2, T3, CancelEventArgs> handler)
	{
		Event -= handler;
	}

	public void RemoveHandler(Func<T1, T2, T3, CancelEventArgs, Task> handler)
	{
		AsyncEvent -= handler;
	}

	public void Invoke(T1 arg1, T2 arg2, T3 arg3)
	{
		if (Event is not null)
		{
			invoker.Invoke(Event, arg1, arg2, arg3);
		}
		if (AsyncEvent is not null)
		{
			invoker.Invoke(AsyncEvent, arg1, arg2, arg3);
		}
	}

	public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3)
	{
		Task? task = null;
		if (AsyncEvent is not null)
		{
			task = invoker.InvokeAsync(AsyncEvent, arg1, arg2, arg3);
		}
		if (Event is not null)
		{
			invoker.Invoke(Event, arg1, arg2, arg3);
		}
		if (task is not null)
		{
			return task;
		}
		return Task.CompletedTask;
	}
}

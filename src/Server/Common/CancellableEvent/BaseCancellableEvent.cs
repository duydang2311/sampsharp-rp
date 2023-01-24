using System.ComponentModel;

namespace Server.Common.CancellableEvent;

public class BaseCancellableEvent : ICancellableEvent
{
	private readonly ICancellableEventInvoker invoker;

	protected event Action<CancelEventArgs>? Event;
	protected event Func<CancelEventArgs, Task>? AsyncEvent;

	public BaseCancellableEvent(ICancellableEventInvoker invoker)
	{
		this.invoker = invoker;
	}

	public void AddHandler(Action<CancelEventArgs> handler)
	{
		Event += handler;
	}

	public void AddHandler(Func<CancelEventArgs, Task> handler)
	{
		AsyncEvent += handler;
	}

	public void RemoveHandler(Action<CancelEventArgs> handler)
	{
		Event -= handler;
	}

	public void RemoveHandler(Func<CancelEventArgs, Task> handler)
	{
		AsyncEvent -= handler;
	}

	public void Invoke()
	{
		if (Event is not null)
		{
			invoker.Invoke(Event);
		}
		if (AsyncEvent is not null)
		{
			invoker.Invoke(AsyncEvent);
		}
	}

	public Task InvokeAsync()
	{
		Task? task = null;
		if (AsyncEvent is not null)
		{
			task = invoker.InvokeAsync(AsyncEvent);
		}
		if (Event is not null)
		{
			invoker.Invoke(Event);
		}
		if (task is not null)
		{
			return task;
		}
		return Task.CompletedTask;
	}
}

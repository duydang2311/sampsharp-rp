namespace Server.Common.Event;

public class BaseEvent : IEvent
{
	private readonly IEventInvoker invoker;

	protected event Action? Event;
	protected event Func<Task>? AsyncEvent;

	public BaseEvent(IEventInvoker invoker)
	{
		this.invoker = invoker;
	}

	public void AddHandler(Action handler)
	{
		Event += handler;
	}

	public void AddHandler(Func<Task> handler)
	{
		AsyncEvent += handler;
	}

	public void RemoveHandler(Action handler)
	{
		Event -= handler;
	}

	public void RemoveHandler(Func<Task> handler)
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

	public Task InvokeAsyncSerial()
	{
		if (Event is not null)
		{
			invoker.Invoke(Event);
		}
		if (AsyncEvent is not null)
		{
			return invoker.InvokeAsyncSerial(AsyncEvent);
		}
		return Task.CompletedTask;
	}
}

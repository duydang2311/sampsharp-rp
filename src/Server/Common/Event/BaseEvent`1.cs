namespace Server.Common.Event;

public class BaseEvent<T1> : IEvent<T1>
{
    private readonly IEventInvoker invoker;

	protected event Action<T1>? Event;
	protected event Func<T1, Task>? AsyncEvent;

    public BaseEvent(IEventInvoker invoker)
    {
        this.invoker = invoker;
    }

	public void AddHandler(Action<T1> handler)
	{
		Event += handler;
	}

	public void AddHandler(Func<T1, Task> handler)
	{
		AsyncEvent += handler;
	}

	public void RemoveHandler(Action<T1> handler)
	{
		Event -= handler;
	}

	public void RemoveHandler(Func<T1, Task> handler)
	{
		AsyncEvent -= handler;
	}

	public void Invoke(T1 arg1)
	{
		if (Event is not null)
		{
            invoker.Invoke(Event, arg1);
		}
		if (AsyncEvent is not null)
		{
            invoker.Invoke(AsyncEvent, arg1);
		}
	}

	public Task InvokeAsync(T1 arg1)
	{
        Task? task = null;
		if (AsyncEvent is not null)
		{
            task = invoker.InvokeAsync(AsyncEvent, arg1);
		}
		if (Event is not null)
		{
            invoker.Invoke(Event, arg1);
		}
		if (task is not null)
		{
			return task;
		}
		return Task.CompletedTask;
	}

	public Task InvokeAsyncSerial(T1 arg1)
	{
		if (Event is not null)
		{
            invoker.Invoke(Event, arg1);
		}
		if (AsyncEvent is not null)
		{
            return invoker.InvokeAsyncSerial(AsyncEvent, arg1);
		}
        return Task.CompletedTask;
	}
}

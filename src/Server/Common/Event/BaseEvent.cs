namespace Server.Common.Event;

public class BaseEvent : IEvent
{
	protected event Action? Event;
	protected event Func<Task>? AsyncEvent;

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
			Event();
		}
		if (AsyncEvent is not null)
		{
			AsyncEvent();
		}
	}

	public Task InvokeAsync()
	{
		var tasks = new LinkedList<Task>();
		if (AsyncEvent is not null)
		{
			foreach (var @delegate in AsyncEvent.GetInvocationList())
			{
				tasks.AddLast((Task)@delegate.Method.Invoke(@delegate.Target, null)!);
			}
		}
		if (Event is not null)
		{
			Event();
		}
		if (AsyncEvent is not null)
		{
			return Task.WhenAll(tasks);
		}
		return Task.CompletedTask;
	}

	public async Task InvokeAsyncSerial()
	{
		if (Event is not null)
		{
			Event();
		}
		if (AsyncEvent is not null)
		{
			foreach (var @delegate in AsyncEvent.GetInvocationList())
			{
				await (Task)@delegate.Method.Invoke(@delegate.Target, null)!;
			}
		}
	}
}

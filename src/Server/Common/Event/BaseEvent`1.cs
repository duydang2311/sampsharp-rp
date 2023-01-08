namespace Server.Common.Event;

public class BaseEvent<T1> : IEvent<T1>
{
	protected event Action<T1>? Event;
	protected event Func<T1, Task>? AsyncEvent;

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
			Event(arg1);
		}
		if (AsyncEvent is not null)
		{
			AsyncEvent(arg1);
		}
	}

	public Task InvokeAsync(T1 arg1)
	{
		var tasks = new LinkedList<Task>();
		if (AsyncEvent is not null)
		{
			var args = new object?[] { arg1 };
			foreach (var @delegate in AsyncEvent.GetInvocationList())
			{
				tasks.AddLast((Task)@delegate.Method.Invoke(@delegate.Target, args)!);
			}
		}
		if (Event is not null)
		{
			Event(arg1);
		}
		if (AsyncEvent is not null)
		{
			return Task.WhenAll(tasks);
		}
		return Task.CompletedTask;
	}

	public async Task InvokeAsyncSerial(T1 arg1)
	{
		if (Event is not null)
		{
			Event(arg1);
		}
		if (AsyncEvent is not null)
		{
			var args = new object?[] { arg1 };
			foreach (var @delegate in AsyncEvent.GetInvocationList())
			{
				await (Task)@delegate.Method.Invoke(@delegate.Target, args)!;
			}
		}
	}
}

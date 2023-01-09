namespace Server.Common.Event;

public class BaseEvent<T1, T2, T3, T4, T5, T6> : IEvent<T1, T2, T3, T4, T5, T6>
{
    protected event Action<T1, T2, T3, T4, T5, T6>? Event;
    protected event Func<T1, T2, T3, T4, T5, T6, Task>? AsyncEvent;

    public void AddHandler(Action<T1, T2, T3, T4, T5, T6> handler)
    {
        Event += handler;
    }

    public void AddHandler(Func<T1, T2, T3, T4, T5, T6, Task> handler)
    {
        AsyncEvent += handler;
    }

    public void RemoveHandler(Action<T1, T2, T3, T4, T5, T6> handler)
    {
        Event -= handler;
    }

    public void RemoveHandler(Func<T1, T2, T3, T4, T5, T6, Task> handler)
    {
        AsyncEvent -= handler;
    }

    public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        if (Event is not null)
        {
            Event(arg1, arg2, arg3, arg4, arg5, arg6);
        }
        if (AsyncEvent is not null)
        {
            AsyncEvent(arg1, arg2, arg3, arg4, arg5, arg6);
        }
    }

    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        var tasks = new LinkedList<Task>();
        if (AsyncEvent is not null)
        {
            var args = new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 };
            foreach (var @delegate in AsyncEvent.GetInvocationList())
            {
                tasks.AddLast((Task)@delegate.Method.Invoke(@delegate.Target, args)!);
            }
        }
        if (Event is not null)
        {
            Event(arg1, arg2, arg3, arg4, arg5, arg6);
        }
        if (AsyncEvent is not null)
        {
            return Task.WhenAll(tasks);
        }
        return Task.CompletedTask;
    }

    public async Task InvokeAsyncSerial(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        if (Event is not null)
        {
            Event(arg1, arg2, arg3, arg4, arg5, arg6);
        }
        if (AsyncEvent is not null)
        {
            var args = new object?[] { arg1, arg2, arg3, arg4, arg5, arg6 };
            foreach (var @delegate in AsyncEvent.GetInvocationList())
            {
                await (Task)@delegate.Method.Invoke(@delegate.Target, args)!;
            }
        }
    }
}

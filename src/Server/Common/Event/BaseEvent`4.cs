namespace Server.Common.Event;

public class BaseEvent<T1, T2, T3, T4> : IEvent<T1, T2, T3, T4>
{
    private readonly IEventInvoker invoker;

    protected event Action<T1, T2, T3, T4>? Event;
    protected event Func<T1, T2, T3, T4, Task>? AsyncEvent;

    public BaseEvent(IEventInvoker invoker)
    {
        this.invoker = invoker;
    }

    public void AddHandler(Action<T1, T2, T3, T4> handler)
    {
        Event += handler;
    }

    public void AddHandler(Func<T1, T2, T3, T4, Task> handler)
    {
        AsyncEvent += handler;
    }

    public void RemoveHandler(Action<T1, T2, T3, T4> handler)
    {
        Event -= handler;
    }

    public void RemoveHandler(Func<T1, T2, T3, T4, Task> handler)
    {
        AsyncEvent -= handler;
    }

    public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (Event is not null)
        {
            invoker.Invoke(Event, arg1, arg2, arg3, arg4);
        }
        if (AsyncEvent is not null)
        {
            invoker.Invoke(AsyncEvent, arg1, arg2, arg3, arg4);
        }
    }

    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        Task? task = null;
        if (AsyncEvent is not null)
        {
            task = invoker.InvokeAsync(AsyncEvent, arg1, arg2, arg3, arg4);
        }
        if (Event is not null)
        {
            invoker.Invoke(Event, arg1, arg2, arg3, arg4);
        }
        if (task is not null)
        {
            return task;
        }
        return Task.CompletedTask;
    }

    public Task InvokeAsyncSerial(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (Event is not null)
        {
            invoker.Invoke(Event, arg1, arg2, arg3, arg4);
        }
        if (AsyncEvent is not null)
        {
            return invoker.InvokeAsyncSerial(AsyncEvent, arg1, arg2, arg3, arg4);
        }
        return Task.CompletedTask;
    }
}

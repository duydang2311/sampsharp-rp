namespace Server.Common.Event;

public class EventInvoker : IEventInvoker
{
    public void Invoke(Delegate @delegate, params object?[]? args)
    {
        foreach(var invocation in @delegate.GetInvocationList())
        {
            invocation.Method.Invoke(invocation.Target, args);
        }
    }

    public Task InvokeAsync(Delegate @delegate, params object?[]? args)
    {
        var tasks = new LinkedList<Task>();
        foreach (var invocation in @delegate.GetInvocationList())
        {
            tasks.AddLast((Task)invocation.Method.Invoke(invocation.Target, args)!);
        }

        return Task.WhenAll(tasks);
    }

    public async Task InvokeAsyncSerial(Delegate @delegate, params object?[]? args)
    {
        foreach (var invocation in @delegate.GetInvocationList())
        {
            await (Task)invocation.Method.Invoke(invocation.Target, args)!;
        }
    }
}

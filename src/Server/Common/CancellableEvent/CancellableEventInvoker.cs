using System.ComponentModel;

namespace Server.Common.CancellableEvent;

public class CancellableEventInvoker : ICancellableEventInvoker
{
	private static object?[] CreateExtendedArgs(object?[]? args, object extendedValue)
	{
		if (args is null)
		{
			return new object?[] { extendedValue };
		}
		var extendedArgs = new object?[args.Length + 1];
		args.CopyTo(extendedArgs, 0);
		extendedArgs[^1] = extendedValue;
		return extendedArgs;
	}

	public void Invoke(Delegate @delegate, params object?[]? args)
	{
		var cancelEventArgs = new CancelEventArgs();
		object?[] extendedArgs = CreateExtendedArgs(args, cancelEventArgs);
		foreach (var invocation in @delegate.GetInvocationList())
		{
			invocation.Method.Invoke(invocation.Target, extendedArgs);
			if (cancelEventArgs.Cancel)
			{
				break;
			}
		}
	}

	public Task InvokeAsync(Delegate @delegate, params object?[]? args)
	{
		return InvokeAsyncSerial(@delegate, args);
	}

	public async Task InvokeAsyncSerial(Delegate @delegate, params object?[]? args)
	{
		var cancelEventArgs = new CancelEventArgs();
		object?[] extendedArgs = CreateExtendedArgs(args, cancelEventArgs);
		foreach (var invocation in @delegate.GetInvocationList())
		{
			await (Task)invocation.Method.Invoke(invocation.Target, extendedArgs)!;
			if (cancelEventArgs.Cancel)
			{
				break;
			}
		}
	}
}

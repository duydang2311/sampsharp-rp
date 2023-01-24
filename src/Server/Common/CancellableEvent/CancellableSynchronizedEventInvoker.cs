using System.ComponentModel;
using SampSharp.Core;

namespace Server.Common.CancellableEvent;

public class CancellableSynchronizedEventInvoker : ICancellableEventInvoker
{
	private readonly ISynchronizationProvider syncProvider;

	public CancellableSynchronizedEventInvoker(ISynchronizationProvider syncProvider)
	{
		this.syncProvider = syncProvider;
	}

	private object?[] CreateExtendedArgs(object?[]? args, object extendedValue)
	{
		if (args is null)
		{
			return new object?[] { extendedValue };
		}
		var extendedArgs = new object?[args.Length + 1];
		args.CopyTo(extendedArgs, 0);
		extendedArgs[extendedArgs.Length - 1] = extendedValue;
		return extendedArgs;
	}

	public void Invoke(Delegate @delegate, params object?[]? args)
	{
		var cancelEventArgs = new CancelEventArgs();
		object?[] extendedArgs = CreateExtendedArgs(args, cancelEventArgs);
		if (syncProvider.InvokeRequired)
		{
			foreach (var invocation in @delegate.GetInvocationList())
			{
				invocation.Method.Invoke(invocation.Target, extendedArgs);
				if (cancelEventArgs.Cancel)
				{
					break;
				}
			}

			return;
		}

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
			if (syncProvider.InvokeRequired)
			{
				var taskCompletionSource = new TaskCompletionSource();
				syncProvider.Invoke(() =>
				{
					(invocation.Method.Invoke(invocation.Target, extendedArgs) as Task)!.ContinueWith(_ =>
					{
						taskCompletionSource.SetResult();
					});
				});
				await taskCompletionSource.Task.ConfigureAwait(false);
				if (cancelEventArgs.Cancel)
				{
					break;
				}
				continue;
			}

			await (Task)invocation.Method.Invoke(invocation.Target, args)!;
			if (cancelEventArgs.Cancel)
			{
				break;
			}
		}
	}
}

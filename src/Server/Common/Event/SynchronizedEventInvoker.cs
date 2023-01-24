using SampSharp.Core;

namespace Server.Common.Event;

public class SynchronizedEventInvoker : IEventInvoker
{
	private readonly ISynchronizationProvider syncProvider;

	public SynchronizedEventInvoker(ISynchronizationProvider syncProvider)
	{
		this.syncProvider = syncProvider;
	}

	public void Invoke(Delegate @delegate, params object?[]? args)
	{
		if (syncProvider.InvokeRequired)
		{
			foreach (var invocation in @delegate.GetInvocationList())
			{
				invocation.Method.Invoke(invocation.Target, args);
			}

			return;
		}

		foreach (var invocation in @delegate.GetInvocationList())
		{
			invocation.Method.Invoke(invocation.Target, args);
		}
	}

	public Task InvokeAsync(Delegate @delegate, params object?[]? args)
	{
		if (syncProvider.InvokeRequired)
		{
			var taskCompletionSource = new TaskCompletionSource();
			syncProvider.Invoke(() =>
			{
				var tasks = new LinkedList<Task>();
				foreach (var invocation in @delegate.GetInvocationList())
				{
					tasks.AddLast((Task)invocation.Method.Invoke(invocation.Target, args)!);
				}

				Task.WhenAll(tasks).ContinueWith(_ => { taskCompletionSource.SetResult(); });
			});
			return taskCompletionSource.Task;
		}

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
			if (syncProvider.InvokeRequired)
			{
				var taskCompletionSource = new TaskCompletionSource();
				syncProvider.Invoke(() =>
				{
					(invocation.Method.Invoke(invocation.Target, args) as Task)!.ContinueWith(_ =>
					{
						taskCompletionSource.SetResult();
					});
				});
				await taskCompletionSource.Task.ConfigureAwait(false);
				continue;
			}

			await (Task)invocation.Method.Invoke(invocation.Target, args)!;
		}
	}
}

using Server.Common.CancellableEvent;
using Server.Common.Event;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithCommon(this IServiceCollection self)
	{
		return self
			.AddSingleton<IEventInvoker, SynchronizedEventInvoker>()
			.AddSingleton<ICancellableEventInvoker, CancellableSynchronizedEventInvoker>();
	}
}

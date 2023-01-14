using Server.Common.Event;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class IServiceCollectionExtensions
{
    public static IServiceCollection WithCommon(this IServiceCollection self)
    {
        return self.AddSingleton<IEventInvoker, SynchronizedEventInvoker>();
    }
}
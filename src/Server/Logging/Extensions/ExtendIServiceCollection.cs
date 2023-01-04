using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ExtendIServiceCollection
{
	public static IServiceCollection WithLogging(this IServiceCollection self)
	{
		return self.AddLogging(configure =>
		{
			configure.AddConsole();
			configure.SetMinimumLevel(LogLevel.Information);
		});
	}
}

using Server.Geometry.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithGeometry(this IServiceCollection self)
	{
		return self
			.AddSingleton<IAreaFactory, AreaFactory>();
	}
}

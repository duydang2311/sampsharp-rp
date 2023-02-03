using Server.SpatialGrid.Entities;
using Server.SpatialGrid.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithSpatialGrid(this IServiceCollection self)
	{
		return self
			.AddSingleton<IGridBuilderFactory, GridBuiderFactory>()
			.AddTransient<ISanAndreasGrid, SanAndreasGrid>();
	}
}

using Server.Vehicles.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithVehicles(this IServiceCollection self)
	{
		return self
			.AddSingleton<IVehicleFactory, VehicleFactory>();
	}
}

using SampSharp.Entities;
using Server.Vehicles.Services;
using Server.Vehicles.Systems;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithVehicles(this IServiceCollection self)
	{
		return self
			.AddSystem<CreateVehicleCommandSystem>()
			.AddSingleton<IVehicleFactory, VehicleFactory>();
	}
}

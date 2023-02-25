using SampSharp.Entities;
using Server.Vehicles.Services;
using Server.Vehicles.Systems;
using Server.Vehicles.Systems.VehicleCommand;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithVehicles(this IServiceCollection self)
	{
		return self
			.AddSystem<CreateVehicleCommandSystem>()
			.AddSystem<InsertVehicleCommandSystem>()
			.AddSystem<LoadDbVehicleSystem>()
			.AddSystem<SaveVehicleInfoSystem>()
			.AddSystem<VehicleCommandSystem>()
			.AddSingleton<IVehicleFactory, VehicleFactory>();
	}
}

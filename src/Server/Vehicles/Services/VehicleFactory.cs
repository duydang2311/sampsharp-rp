using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.Vehicles.Services;

public sealed class VehicleFactory : IVehicleFactory
{
	private readonly IWorldService worldService;

	public VehicleFactory(IWorldService worldService)
	{
		this.worldService = worldService;
	}

	public Vehicle? CreateVehicle(VehicleModelType type, Vector3 position, float rotation, int color1, int color2, int respawnDelay = -1, bool addSiren = false)
	{
		try
		{
			var vehicle = worldService.CreateVehicle(type, position, rotation, color1, color2, respawnDelay, addSiren);
			return vehicle;
		}
		catch (EntityCreationException)
		{
			return default;
		}
	}
}

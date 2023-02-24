using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Vehicles.Components;

namespace Server.Vehicles.Services;

public sealed class VehicleFactory : IVehicleFactory
{
	private readonly IWorldService worldService;
	private readonly IEntityManager entityManager;

	public VehicleFactory(IWorldService worldService, IEntityManager entityManager)
	{
		this.worldService = worldService;
		this.entityManager = entityManager;
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

	public Vehicle? GetVehicle(EntityId entityId)
	{
		return entityManager.GetComponent<Vehicle>(entityId);
	}

	public Vehicle? GetVehicle(int vehicleId)
	{
		return GetVehicle(SampEntities.GetVehicleId(vehicleId));
	}

	public IReadOnlyCollection<Vehicle> GetVehicles()
	{
		return entityManager.GetComponents<Vehicle>();
	}

	public IReadOnlyCollection<VehicleComponent> GetVehicleComponents()
	{
		return entityManager.GetComponents<VehicleComponent>();
	}
}

using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Vehicles.Components;
using Server.Vehicles.Entities;

namespace Server.Vehicles.Services;

public sealed class VehicleFactory : IVehicleFactory
{
	private readonly IEntityManager entityManager;
	private readonly WorldServiceNative worldServiceNative;

	public VehicleFactory(IEntityManager entityManager, WorldServiceNative worldServiceNative)
	{
		this.entityManager = entityManager;
		this.worldServiceNative = worldServiceNative;
	}

	public Vehicle? CreateVehicle(VehicleModelType type, Vector3 position, float rotation, int interior, int world, int color1, int color2, int respawnDelay = -1, bool addSiren = false)
	{
		var id = worldServiceNative.CreateVehicle((int)type, position.X, position.Y, position.Z, rotation, color1, color2, respawnDelay, addSiren);

		if (id == NativeVehicle.InvalidId)
		{
			return default;
		}
		var entity = SampEntities.GetVehicleId(id);
		entityManager.Create(entity, EntityId.Empty);

		var vehicle = new ServerVehicle()
		{
			Interior = interior,
			VirtualWorld = world,
			// TODO: reduce 2 native calls here
			PrimaryColor = color1,
			SecondaryColor = color2,
		};
		entityManager.AddComponent(entity, new NativeVehicle());
		entityManager.AddComponent<Vehicle>(entity, vehicle);
		return vehicle;
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

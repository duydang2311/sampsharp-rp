using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Vehicles.Components;

namespace Server.Vehicles.Services;

public interface IVehicleFactory
{
	Vehicle? CreateVehicle(VehicleModelType type, Vector3 position, float rotation, int color1, int color2, int respawnDelay = -1, bool addSiren = false);
	Vehicle? GetVehicle(EntityId entityId);
	Vehicle? GetVehicle(int vehicleid);
	IReadOnlyCollection<Vehicle> GetVehicles();
	IReadOnlyCollection<VehicleComponent> GetVehicleComponents();
}

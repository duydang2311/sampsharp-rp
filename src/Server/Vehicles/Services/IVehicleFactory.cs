using SampSharp.Entities.SAMP;

namespace Server.Vehicles.Services;

public interface IVehicleFactory
{
	Vehicle? CreateVehicle(VehicleModelType type, Vector3 position, float rotation, int color1, int color2, int respawnDelay = -1, bool addSiren = false);
}

using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.Vehicles.Systems.VehicleCommand;

public sealed partial class VehicleCommandSystem : ISystem
{
	private void ToggleVehicleLock(Player player)
	{
		Vehicle closestVehicle = null!;
		if (player.InAnyVehicle)
		{
			if (player.VehicleSeat != 0)
			{
				chatService.SendMessage(player, b => b
					.Add(t => t.Badge_Error)
					.Inline(t => t.VehicleCommand_NotDrivingAnyVehicle));
				return;
			}
			closestVehicle = vehicleFactory.GetVehicle(player.Vehicle)!;
		}
		else
		{
			float minDistanceSquared = 5 + float.Epsilon;
			foreach (var v in vehicleFactory.GetVehicles())
			{
				var distanceSquared = Vector3.DistanceSquared(v.Position, player.Position);
				if (distanceSquared < minDistanceSquared)
				{
					minDistanceSquared = distanceSquared;
					closestVehicle = v;
				}
			}
			if (closestVehicle is null)
			{
				chatService.SendMessage(player, b => b
					.Add(t => t.Badge_Error)
					.Inline(t => t.VehicleCommand_NoNearbyVehicle));
				return;
			}
		}

		closestVehicle.Doors = !closestVehicle.Doors;
		if (closestVehicle.Doors)
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Success)
				.Inline(t => t.VehicleCommand_Locked));
		}
		else
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Success)
				.Inline(t => t.VehicleCommand_Unlocked));
		}
	}
}

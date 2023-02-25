using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.Vehicles.Systems.VehicleCommand;

public sealed partial class VehicleCommandSystem : ISystem
{
	private void ToggleVehicleHood(Player player)
	{
		if (!player.InAnyVehicle && player.VehicleSeat != 0)
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Error)
				.Inline(t => t.VehicleCommand_NotDrivingAnyVehicle));
			return;
		}

		var vehicle = vehicleFactory.GetVehicle(player.Vehicle)!;
		vehicle.Bonnet = !vehicle.Bonnet;

		if (vehicle.Bonnet)
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Success)
				.Inline(t => t.VehicleCommand_Hood_Opened));
		}
		else
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Success)
				.Inline(t => t.VehicleCommand_Hood_Closed));
		}
	}
}

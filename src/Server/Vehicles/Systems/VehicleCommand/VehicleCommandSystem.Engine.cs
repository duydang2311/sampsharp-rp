using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.Vehicles.Systems.VehicleCommand;

public sealed partial class VehicleCommandSystem : ISystem
{
	private void ToggleVehicleEngine(Player player)
	{
		if (!player.InAnyVehicle && player.VehicleSeat != 0)
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Error)
				.Inline(t => t.VehicleCommand_NotDrivingAnyVehicle));
			return;
		}

		var vehicle = vehicleFactory.GetVehicle(player.Vehicle)!;
		vehicle.Engine = !vehicle.Engine;

		if (vehicle.Engine)
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Success)
				.Inline(t => t.VehicleCommand_Engine_On));
		}
		else
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Success)
				.Inline(t => t.VehicleCommand_Engine_Off));
		}
	}
}

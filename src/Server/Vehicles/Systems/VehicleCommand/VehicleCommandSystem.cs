using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;
using Server.Vehicles.Services;

namespace Server.Vehicles.Systems.VehicleCommand;

public sealed partial class VehicleCommandSystem : ISystem
{
	private readonly IChatService chatService;
	private readonly IVehicleFactory vehicleFactory;

	public VehicleCommandSystem(ICommandService commandService, IChatService chatService, IVehicleFactory vehicleFactory)
	{
		this.chatService = chatService;
		this.vehicleFactory = vehicleFactory;

		commandService.RegisterCommand(m =>
		{
			m.Name = "vehicle";
			m.Delegate = HandleVehicleCommand;
			m.HelpDelegate = HelpVehicleCommand;
		});
		commandService.RegisterAlias("vehicle", "v");
	}

	private void HelpVehicleCommand(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Help)
			.Inline(t => t.VehicleCommand_Help)
			.Add(t => t.Badge_Help)
			.Inline(t => t.VehicleCommand_Options));
	}

	private void HandleVehicleCommand(Player player, string option, string argument = "")
	{
		switch (option.ToLowerInvariant())
		{
			case "engine":
				{
					ToggleVehicleEngine(player);
					break;
				}
			case "lights":
				{
					ToggleVehicleLights(player);
					break;
				}
			case "hood":
				{
					ToggleVehicleHood(player);
					break;
				}
			case "trunk":
				{
					ToggleVehicleTrunk(player);
					break;
				}
			case "lock":
				{
					ToggleVehicleLock(player);
					break;
				}
			default:
				{
					HelpVehicleCommand(player);
					break;
				}
		}
	}
}

using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Components;
using Server.Chat.Services;
using Server.Vehicles.Services;

namespace Server.Vehicles.Systems.CreateVehicleCommand;

public sealed class CreateVehicleCommandSystem : ISystem
{
	private readonly IChatService chatService;
	private readonly IVehicleFactory vehicleFactory;

	public CreateVehicleCommandSystem(ICommandService commandService, IChatService chatService, IVehicleFactory vehicleFactory)
	{
		this.chatService = chatService;
		this.vehicleFactory = vehicleFactory;

		commandService.RegisterCommand(m =>
		{
			m.Name = "createvehicle";
			m.PermissionLevel = PermissionLevel.Admin;
			m.Delegate = CreateVehicle;
		});
		commandService.RegisterHelper("createvehicle", HelpCreateVehicle);
	}

	private void HelpCreateVehicle(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Help)
			.Inline(t => t.CreateVehicleCommand_Help));
	}

	private void CreateVehicle(Player player, int model, int primaryColor = 1, int secondaryColor = 1, bool addSiren = false)
	{
		var eModel = (VehicleModelType)model;
		if (!Enum.IsDefined<VehicleModelType>((VehicleModelType)model))
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Error)
				.Inline(t => t.CreateVehicleCommand_InvalidModel, model));
			return;
		}


		var vehicle = vehicleFactory.CreateVehicle(eModel, player.Position, player.Angle, primaryColor, secondaryColor, addSiren: addSiren);
		if (vehicle is null)
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Error)
				.Inline(t => t.CreateVehicleCommand_Error));
			return;
		}

		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Success)
			.Inline(t => t.CreateVehicleCommand_Success, vehicle.Entity.Handle));
	}
}

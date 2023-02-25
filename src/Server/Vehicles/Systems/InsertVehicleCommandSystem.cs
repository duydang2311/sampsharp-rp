using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Components;
using Server.Chat.Services;
using Server.Database;
using Server.Vehicles.Components;
using Server.Vehicles.Entities;
using Server.Vehicles.Models;
using Server.Vehicles.Services;

namespace Server.Vehicles.Systems;

public sealed class InsertVehicleCommandSystem : ISystem
{
	private readonly IChatService chatService;
	private readonly IVehicleFactory vehicleFactory;
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;

	public InsertVehicleCommandSystem(ICommandService commandService, IChatService chatService, IVehicleFactory vehicleFactory, IDbContextFactory<ServerDbContext> dbContextFactory)
	{
		this.chatService = chatService;
		this.vehicleFactory = vehicleFactory;
		this.dbContextFactory = dbContextFactory;

		commandService.RegisterCommand(m =>
		{
			m.Name = "insertvehicle";
			m.PermissionLevel = PermissionLevel.Admin;
			m.Delegate = InsertVehicleAsync;
		});
		commandService.RegisterHelper("insertvehicle", HelpInsertVehicle);
	}

	private void HelpInsertVehicle(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Help)
			.Inline(t => t.InsertVehicleCommand_Help));
	}

	private async Task InsertVehicleAsync(Player player, int vehicleid)
	{
		var vehicle = (ServerVehicle?)vehicleFactory.GetVehicle(vehicleid);
		if (vehicle is null)
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Error)
				.Inline(t => t.InsertVehicleCommand_IdNotFound, vehicleid));
			return;
		}

		using var ctx = dbContextFactory.CreateDbContext();
		var model = new VehicleModel
		{
			Model = vehicle.Model,
			X = vehicle.Position.X,
			Y = vehicle.Position.Y,
			Z = vehicle.Position.Z,
			A = vehicle.Angle,
			Interior = vehicle.Interior,
			World = vehicle.VirtualWorld,
			Health = vehicle.Health,
		};
		ctx.Vehicles.Add(model);
		await ctx.SaveChangesAsync();
		if (model.Id == 0)
		{
			chatService.SendMessage(player, b => b
				.Add(t => t.Badge_Error)
				.Inline(t => t.InsertVehicleCommand_Error, vehicleid));
			return;
		}
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Success)
			.Inline(t => t.InsertVehicleCommand_Success, vehicleid, model.Id));
		vehicle.AddComponent(new VehicleComponent { Id = model.Id });
		return;
	}
}

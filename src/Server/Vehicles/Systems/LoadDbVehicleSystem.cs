using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Database;
using Server.Vehicles.Components;
using Server.Vehicles.Services;

namespace Server.Vehicles.Systems;

public sealed partial class LoadDbVehicleSystem : ISystem
{
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly IVehicleFactory vehicleFactory;
	private readonly ILogger<LoadDbVehicleSystem> logger;

	public LoadDbVehicleSystem(IDbContextFactory<ServerDbContext> dbContextFactory, IVehicleFactory vehicleFactory, ILogger<LoadDbVehicleSystem> logger)
	{
		this.dbContextFactory = dbContextFactory;
		this.vehicleFactory = vehicleFactory;
		this.logger = logger;
	}

	[Event]
	private void OnGameModeInit()
	{
		using var ctx = dbContextFactory.CreateDbContext();
		var count = 0;
		foreach (var m in ctx.Vehicles
			.Where(m => m.Character == null)
			.Select(m => new
			{
				m.Id,
				m.Model,
				m.X,
				m.Y,
				m.Z,
				m.A,
				m.Interior,
				m.World,
				m.Health,
				m.PrimaryColor,
				m.SecondaryColor
			}))
		{
			var vehicle = vehicleFactory.CreateVehicle(m.Model, new Vector3(m.X, m.Y, m.Z), m.A, m.Interior, m.World, m.PrimaryColor, m.SecondaryColor);
			if (vehicle is null)
			{
				LogVehicleWithInvalidModel(logger, m.Id, (int)m.Model);
				continue;
			}
			vehicle.AddComponent(new VehicleComponent { Id = m.Id });
			++count;
		}
		LogNumberOfCreatedVehicles(logger, count);
	}

	[LoggerMessage(
		EventId = 0,
		Level = LogLevel.Warning,
		Message = "Unable to create vehicle {Id} due to invalid model {Model}")]
	public static partial void LogVehicleWithInvalidModel(ILogger<LoadDbVehicleSystem> logger, long id, int model);

	[LoggerMessage(
		EventId = 1,
		Level = LogLevel.Information,
		Message = "Loaded {Count} vehicles from database.")]
	public static partial void LogNumberOfCreatedVehicles(ILogger<LoadDbVehicleSystem> logger, int count);
}

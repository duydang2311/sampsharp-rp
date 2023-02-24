using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Database;
using Server.Vehicles.Components;
using Server.Vehicles.Services;

namespace Server.Vehicles.Systems;

public sealed class LoadDbVehicleSystem : ISystem
{
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly IVehicleFactory vehicleFactory;

	public LoadDbVehicleSystem(IDbContextFactory<ServerDbContext> dbContextFactory, IVehicleFactory vehicleFactory)
	{
		this.dbContextFactory = dbContextFactory;
		this.vehicleFactory = vehicleFactory;
	}

	[Event]
	private void OnGameModeInit()
	{
		using var ctx = dbContextFactory.CreateDbContext();
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
			var vehicle = vehicleFactory.CreateVehicle(m.Model, new Vector3(m.X, m.Y, m.Z), m.A, m.PrimaryColor, m.SecondaryColor);
			if (vehicle is null)
			{
				continue;
			}
			vehicle.AddComponent(new VehicleComponent { Id = m.Id });
		}
	}
}

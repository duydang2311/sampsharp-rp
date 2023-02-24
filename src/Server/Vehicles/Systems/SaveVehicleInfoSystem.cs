using System.Text;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SampSharp.Entities;
using Server.Database;
using Server.Vehicles.Services;

namespace Server.Vehicles.Systems;

public sealed class SaveVehicleInfoSystem : ISystem
{
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly IVehicleFactory vehicleFactory;

	public SaveVehicleInfoSystem(IDbContextFactory<ServerDbContext> dbContextFactory, IVehicleFactory vehicleFactory)
	{
		this.dbContextFactory = dbContextFactory;
		this.vehicleFactory = vehicleFactory;
	}

	[Event]
	private async void OnGameModeExit()
	{
		var stringBuilder = new StringBuilder(512);
		var count = 0;
		var parameters = new LinkedList<NpgsqlParameter>();
		foreach (var c in vehicleFactory.GetVehicleComponents())
		{
			var vehicle = vehicleFactory.GetVehicle(c.Entity);
			if (vehicle is null)
			{
				continue;
			}
			stringBuilder.AppendFormat($"UPDATE \"Vehicles\" SET \"Health\" = @Health{0} WHERE \"Id\" = @Id{0};", count);
			parameters.AddLast(new NpgsqlParameter($"Health{count}", vehicle.Health));
			parameters.AddLast(new NpgsqlParameter($"Id{count}", c.Id));
			++count;
		}
		if (stringBuilder.Length == 0)
		{
			return;
		}
		using var ctx = dbContextFactory.CreateDbContext();
		await ctx.Database
			.ExecuteSqlRawAsync(stringBuilder.ToString(), parameters)
			.ConfigureAwait(false);
	}
}

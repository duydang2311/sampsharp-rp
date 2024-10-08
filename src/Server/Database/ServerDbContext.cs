using Microsoft.EntityFrameworkCore;
using Server.Account.Models;
using Server.Character.Models;
using Server.Door.Models;
using Server.Vehicles.Models;

namespace Server.Database;

public sealed class ServerDbContext : DbContext
{
	public DbSet<CharacterModel> Characters => Set<CharacterModel>();
	public DbSet<AccountModel> Accounts => Set<AccountModel>();
	public DbSet<DoorModel> Doors => Set<DoorModel>();
	public DbSet<VehicleModel> Vehicles => Set<VehicleModel>();

	public ServerDbContext() : base() { }
	public ServerDbContext(DbContextOptions options) : base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (Environment.GetEnvironmentVariable("OPTIMIZE_EF") is not null)
		{
			optionsBuilder.UseNpgsql();
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.BuildDoor();
		modelBuilder.BuildVehicle();
	}
}

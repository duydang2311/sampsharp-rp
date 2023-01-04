using Microsoft.EntityFrameworkCore;
using Server.Account.Models;
using Server.Character.Models;
using Server.Door.Models;
using Server.Property.Models;
namespace Server.Database;

public sealed class ServerDbContext : DbContext
{
	public DbSet<CharacterModel> Characters => Set<CharacterModel>();
	public DbSet<AccountModel> Accounts => Set<AccountModel>();
	public DbSet<DoorModel> Doors => Set<DoorModel>();
	public DbSet<PropertyModel> Properties => Set<PropertyModel>();
	public DbSet<PropertyPointModel> PropertyPoints => Set<PropertyPointModel>();

	public ServerDbContext() : base() { }
	public ServerDbContext(DbContextOptions options) : base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// only use when run with efcore optimize
		// optionsBuilder.UseNpgsql();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.BuildDoor();
		modelBuilder.BuildProperty();
	}
}

using Microsoft.EntityFrameworkCore;
using Server.Character.Models;

namespace Server.Database;

public sealed class ServerDbContext : DbContext
{
	public DbSet<CharacterModel> CharacterModels => Set<CharacterModel>();
	public DbSet<CharacterAuthModel> CharacterAuthModels => Set<CharacterAuthModel>();
	public DbSet<CharacterSpawnModel> CharacterSpawnModels => Set<CharacterSpawnModel>();

	public ServerDbContext(DbContextOptions options) : base(options) { }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
}

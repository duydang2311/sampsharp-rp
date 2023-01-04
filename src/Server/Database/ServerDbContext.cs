using Microsoft.EntityFrameworkCore;
using Server.Character.Models;

namespace Server.Database;

public sealed class ServerDbContext : DbContext
{
	public DbSet<CharacterModel> Characters => Set<CharacterModel>();

	public ServerDbContext(DbContextOptions options) : base(options) { }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
}

using Microsoft.EntityFrameworkCore;
using Server.Account.Models;
using Server.Character.Models;

namespace Server.Database;

public sealed class ServerDbContext : DbContext
{
	public DbSet<CharacterModel> Characters => Set<CharacterModel>();
	public DbSet<AccountModel> Accounts => Set<AccountModel>();

	public ServerDbContext(DbContextOptions options) : base(options) { }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
}

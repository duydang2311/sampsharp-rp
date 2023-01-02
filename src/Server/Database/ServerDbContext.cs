using Microsoft.EntityFrameworkCore;

namespace Server.Database;

public sealed class ServerDbContext : DbContext, IServerDbContext
{
	public ServerDbContext(DbContextOptions options) : base(options) { }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
}

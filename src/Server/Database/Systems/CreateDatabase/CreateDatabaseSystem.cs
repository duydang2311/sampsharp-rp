using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;

namespace Server.Database.Systems.CreateDatabase;

public sealed class CreateDatabaseSystem : ISystem
{
	public CreateDatabaseSystem(IDbContextFactory<ServerDbContext> dbContextFactory)
	{
		var ctx = dbContextFactory.CreateDbContext();
		ctx.Database.EnsureDeleted();
		ctx.Database.EnsureCreated();
	}
}

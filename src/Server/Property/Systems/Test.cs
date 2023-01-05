using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using Server.Database;

namespace Server.Property.Systems;

public sealed class Test : ISystem
{
	public Test(IDbContextFactory<ServerDbContext> factory)
	{
		using var ctx = factory.CreateDbContext();
		ctx.Database.EnsureDeleted();
		ctx.Database.EnsureCreated();
	}
}

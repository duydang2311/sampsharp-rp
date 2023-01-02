using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Microsoft.Extensions.DependencyInjection;

public static class ExtendIServiceCollection
{
	public static IServiceCollection AddDatabase(this IServiceCollection self)
	{
		var host = Environment.GetEnvironmentVariable("host");
		var username = Environment.GetEnvironmentVariable("username");
		var password = Environment.GetEnvironmentVariable("password");
		var database = Environment.GetEnvironmentVariable("database");
		if (host is null
		|| username is null
		|| password is null
		|| database is null)
		{
			throw new Exception("missing db connection credentials");
		}
		return self.AddDbContext<IServerDbContext, ServerDbContext>((optionsBuilder) =>
		{
			optionsBuilder.UseNpgsql($"Host={host};Username={username};Password={password};Database={database}");
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		});
	}
}

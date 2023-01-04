using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ExtendIServiceCollection
{
	public static IServiceCollection WithDatabase(this IServiceCollection self)
	{
		DotEnv.Load(options: new DotEnvOptions(overwriteExistingVars: false));
		var host = Environment.GetEnvironmentVariable("host");
		var username = Environment.GetEnvironmentVariable("username");
		var password = Environment.GetEnvironmentVariable("password");
		var database = Environment.GetEnvironmentVariable("database");
		var port = Environment.GetEnvironmentVariable("port") ?? "5432";
		if (host is null
		|| username is null
		|| password is null
		|| database is null)
		{
			throw new Exception("missing db connection credentials");
		}
		return self.AddPooledDbContextFactory<ServerDbContext>(optionsAction =>
		{
			optionsAction.UseNpgsql($"Host={host};Username={username};Password={password};Database={database};Port={port}");
			optionsAction.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}, 256);
	}
}

using dotenv.net;
using dotenv.net.Utilities;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class IServiceCollectionExtensions
{
	public static IServiceCollection WithDatabase(this IServiceCollection self)
	{
		DotEnv.Load(options: new DotEnvOptions(overwriteExistingVars: false));
		var envVars = DotEnv.Read();
		if (!EnvReader.HasValue("host")
		|| !EnvReader.HasValue("username")
		|| !EnvReader.HasValue("password")
		|| !EnvReader.HasValue("database"))
		{
			throw new Exception("missing db connection credentials");
		}
		var host = envVars["host"];
		var username = envVars["username"];
		var password = envVars["password"];
		var database = envVars["database"];
		var port = EnvReader.HasValue("port") ? envVars["port"] : "5432";

		return self.AddPooledDbContextFactory<ServerDbContext>(optionsAction =>
		{
			optionsAction.UseNpgsql($"Host={host};Username={username};Password={password};Database={database};Port={port}");
			optionsAction.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			optionsAction.EnableDetailedErrors();
#if DEBUG
			optionsAction.EnableSensitiveDataLogging();
#endif
		}, 256);
	}
}

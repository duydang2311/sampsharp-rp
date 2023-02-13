using dotenv.net;
using dotenv.net.Utilities;
using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using Server.Database;
using Server.Database.Systems.CreateDatabase;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
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

		self
			.AddPooledDbContextFactory<ServerDbContext>(optionsAction =>
			{
				optionsAction
					.UseNpgsql($"Host={host};Username={username};Password={password};Database={database};Port={port}")
					.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
					.EnableDetailedErrors()
#if DEBUG
					.EnableSensitiveDataLogging()
#endif
					.UseModel(Database.CompiledModels.ServerDbContextModel.Instance);
			}, 256);
		if (Environment.GetEnvironmentVariable("CREATE_DB") is not null)
		{
			self.AddSystem<CreateDatabaseSystem>();
		}
		return self;
	}
}

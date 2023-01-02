using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Tests.Database;

public class TestEnsureCreated
{
	private IServiceProvider provider = null!;
	private class TestDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var host = Environment.GetEnvironmentVariable("host");
			var username = Environment.GetEnvironmentVariable("username");
			var password = Environment.GetEnvironmentVariable("password");
			var database = Environment.GetEnvironmentVariable("database");
			var port = Environment.GetEnvironmentVariable("port") ?? "5432";
			optionsBuilder.UseNpgsql($"Host={host};Username={username};Password={password};Database={database};Port={port};sslmode=Prefer");
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}
	}

	[SetUp]
	public void Setup()
	{
		var serviceCollection = new ServiceCollection();
		DotEnv.Load(new DotEnvOptions(envFilePaths: new string[] { Path.Combine(Environment.CurrentDirectory[..Environment.CurrentDirectory.IndexOf("bin")], @"../../.env") }));
		serviceCollection.AddDbContext<TestDbContext>();
		provider = serviceCollection.BuildServiceProvider();
	}

	[Test]
	public void Database_EnsureCreated_ReturnsTrue()
	{
		Assert.That(provider.GetRequiredService<TestDbContext>().Database.EnsureCreated(), Is.True);
	}
}

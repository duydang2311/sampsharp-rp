using dotenv.net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Tests.Helper;

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
		if (EnvironmentHelper.IsOnCI)
		{
			return;
		}
		var serviceCollection = new ServiceCollection();
		DotEnv
			.Fluent()
			.WithOverwriteExistingVars()
			.WithEnvFiles(Path.Combine(Environment.CurrentDirectory[..Environment.CurrentDirectory.IndexOf("bin")], @"../../.env"))
			.Load();
		serviceCollection.AddDbContext<TestDbContext>();
		provider = serviceCollection.BuildServiceProvider();
	}

	[Test]
	public void Database_EnsureCreated_ReturnsTrue()
	{
		if (EnvironmentHelper.IsOnCI)
		{
			Assert.Ignore("CI");
			return;
		}
		provider.GetRequiredService<TestDbContext>().Database.EnsureCreated().Should().BeTrue();
	}
}

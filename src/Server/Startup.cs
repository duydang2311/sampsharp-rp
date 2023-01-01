using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server
{
	public class Startup : IStartup
	{
		public void Configure(IServiceCollection services)
		{
			services
				.AddSystemsInAssembly();
		}

		public void Configure(IEcsBuilder builder)
		{
			builder.EnableSampEvents() // Enable all stock SA-MP callbacks as events which can be listened to by systems.
				.EnablePlayerCommands(); // Enable player commands being loaded in systems.
		}
	}
}

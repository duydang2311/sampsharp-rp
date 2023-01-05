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
				.WithDatabase()
				.WithLogging()
				.WithChat()
				.AddSystemsInAssembly();
		}

		public void Configure(IEcsBuilder builder)
		{
			builder
				.UseChatMiddlewares()
				.EnableSampEvents();
		}
	}
}

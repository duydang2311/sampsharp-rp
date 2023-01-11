using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
				.WithI18n((globalOptions, textLocalizerOptions) =>
				{
					globalOptions.AddCulture("vi");
					globalOptions.AddCulture("vi-VN");
					textLocalizerOptions.BaseName = "Server.Resources.Text";
					textLocalizerOptions.DefaultCulture = CultureInfo.GetCultureInfo("vi");
				})
				.WithSAMP();

			var provider = services.BuildServiceProvider();
			var logger = provider.GetRequiredService<ILogger<Startup>>();
			foreach (var service in services)
			{
				if (service.Lifetime != ServiceLifetime.Singleton)
				{
					continue;
				}
				var serviceType = service.ServiceType;
				var implType = service.ImplementationType;
				if (serviceType.Namespace!.StartsWith("Server")
				&& serviceType.IsAssignableTo(typeof(ISystem))
				|| (implType is not null
					&& implType.IsAssignableTo(typeof(ISystem))))
				{
					logger.LogInformation("Enabled system {System}", serviceType.FullName);
					provider.GetRequiredService(serviceType);
				}
			}
		}

		public void Configure(IEcsBuilder builder)
		{
			builder
				.UseChatMiddlewares()
				.EnableSampEvents();
		}
	}
}

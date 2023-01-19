using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server
{
	public class Startup : IStartup
	{
		public void Configure(IServiceCollection services)
		{
			_ = services
				.WithDatabase()
				.WithLogging()
				.WithChat()
				.WithAccount()
				.WithI18N((globalOptions, textLocalizerOptions) =>
				{
					globalOptions.AddCulture("vi");
					globalOptions.AddCulture("vi-VN");
					textLocalizerOptions.BaseName = "Server.Resources.Text";
					textLocalizerOptions.DefaultCulture = CultureInfo.GetCultureInfo("vi");
				})
				.WithSAMP()
				.WithCommon()
				.WithDoor();
		}

		public void Configure(IEcsBuilder builder)
		{
			_ = builder
				.UseChatMiddlewares()
				.EnableSampEvents();
		}
	}
}

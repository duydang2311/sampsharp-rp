using SampSharp.Entities;
using SampSharp.Streamer.Entities;
using Server.SAMP.Dialog.Services;
using Server.SAMP.Streamer.Systems.ItemUpdate;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithSAMP(this IServiceCollection self)
	{
		return self
			.AddSystem<ToggleItemUpdateSystem>()
			.AddSingleton<ICustomDialogFactory, CustomDialogFactory>()
			.AddSingleton<ICustomDialogService, CustomDialogService>()
			.AddSingleton<IStreamerService, StreamerService>();
	}
}

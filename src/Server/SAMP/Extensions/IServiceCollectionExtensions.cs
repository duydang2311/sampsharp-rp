
using Server.SAMP.Dialog.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class IServiceCollectionExtensions
{
	public static IServiceCollection WithSAMP(this IServiceCollection self)
	{
		return self
			.AddSingleton<ICustomDialogFactory, CustomDialogFactory>()
			.AddSingleton<ICustomDialogService, CustomDialogService>();
	}
}


using Server.SAMP.Dialog.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ExtendIServiceCollection
{
	public static IServiceCollection WithSAMP(this IServiceCollection self)
	{
		return self
			.AddSingleton<IDialogFactory, DialogFactory>()
			.AddSingleton<IDialogService, DialogService>();
	}
}

using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public sealed class CustomDialogService : ICustomDialogService
{
	private readonly ICustomDialogFactory dialogFactory;
	private readonly IDialogService sampSharpDialogService;

	public CustomDialogService(ICustomDialogFactory dialogFactory, IDialogService sampSharpDialogService)
	{
		this.dialogFactory = dialogFactory;
		this.sampSharpDialogService = sampSharpDialogService;
	}

	public Task<TResponse> ShowAsync<TResponse>(EntityId player, Func<ICustomDialogFactory, IDialog<TResponse>> dialogCreator)
	where TResponse : struct
	{
		return ShowAsync(player, dialogCreator(dialogFactory));
	}

	public void Show<TResponse>(EntityId player, Func<ICustomDialogFactory, IDialog<TResponse>> dialogCreator, Action<TResponse> responseHandler)
	where TResponse : struct
	{
		Show(player, dialogCreator(dialogFactory), responseHandler);
	}

	public Task<TResponse> ShowAsync<TResponse>(EntityId player, IDialog<TResponse> dialog)
	where TResponse : struct
	{
		return sampSharpDialogService.Show(player, dialog);
	}

	public void Show<TResponse>(EntityId player, IDialog<TResponse> dialog, Action<TResponse> responseHandler)
	where TResponse : struct
	{
		sampSharpDialogService.Show(player, dialog, responseHandler);
	}
}

using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public sealed class CustomDialogService : ICustomDialogService
{
	private readonly IDialogBuilderFactory dialogBuilderFactory;
	private readonly IDialogService sampSharpDialogService;

	public CustomDialogService(IDialogBuilderFactory dialogBuilderFactory, IDialogService sampSharpDialogService)
	{
		this.dialogBuilderFactory = dialogBuilderFactory;
		this.sampSharpDialogService = sampSharpDialogService;
	}

	public Task<TResponse> ShowAsync<TResponse>(EntityId player, Func<IDialogBuilderFactory, IDialog<TResponse>> buildDialog)
	where TResponse : struct
	{
		return ShowAsync(player, buildDialog(dialogBuilderFactory));
	}

	public void Show<TResponse>(EntityId player, Func<IDialogBuilderFactory, IDialog<TResponse>> buildDialog, Action<TResponse> responseHandler)
	where TResponse : struct
	{
		Show(player, buildDialog(dialogBuilderFactory), responseHandler);
	}

	public Task<TResponse> ShowAsync<TResponse>(EntityId player, IDialog<TResponse> dialog)
	where TResponse : struct
	{
		return sampSharpDialogService.ShowAsync(player, dialog);
	}

	public void Show<TResponse>(EntityId player, IDialog<TResponse> dialog, Action<TResponse> responseHandler)
	where TResponse : struct
	{
		sampSharpDialogService.Show(player, dialog, responseHandler);
	}
}

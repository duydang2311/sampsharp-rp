using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

using ISampSharpDialogService = SampSharp.Entities.SAMP.IDialogService;

public sealed class DialogService : IDialogService
{
	private readonly IDialogFactory dialogFactory;
	private readonly ISampSharpDialogService sampSharpDialogService;

	public DialogService(IDialogFactory dialogFactory, ISampSharpDialogService sampSharpDialogService)
	{
		this.dialogFactory = dialogFactory;
		this.sampSharpDialogService = sampSharpDialogService;
	}

	public Task<TResponse> Show<TResponse>(EntityId player, Func<IDialogFactory, IDialog<TResponse>> dialogCreator)
	where TResponse : struct
	{
		return Show(player, dialogCreator(dialogFactory));
	}

	public void Show<TResponse>(EntityId player, Func<IDialogFactory, IDialog<TResponse>> dialogCreator, Action<TResponse> responseHandler)
	where TResponse : struct
	{
		Show(player, dialogCreator(dialogFactory), responseHandler);
	}

	public Task<TResponse> Show<TResponse>(EntityId player, IDialog<TResponse> dialog)
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

using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public interface IDialogService
{
	Task<TResponse> Show<TResponse>(EntityId player, Func<IDialogFactory, IDialog<TResponse>> dialogCreator)
	where TResponse : struct;
	void Show<TResponse>(EntityId player, Func<IDialogFactory, IDialog<TResponse>> dialogCreator, Action<TResponse> responseHandler)
	where TResponse : struct;
	Task<TResponse> Show<TResponse>(EntityId player, IDialog<TResponse> dialog)
	where TResponse : struct;
	void Show<TResponse>(EntityId player, IDialog<TResponse> dialog, Action<TResponse> responseHandler)
	where TResponse : struct;
}

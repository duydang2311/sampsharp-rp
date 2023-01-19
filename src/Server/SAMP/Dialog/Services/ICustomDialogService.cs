using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public interface ICustomDialogService
{
	Task<TResponse> ShowAsync<TResponse>(EntityId player, Func<ICustomDialogFactory, IDialog<TResponse>> dialogCreator)
	where TResponse : struct;
	void Show<TResponse>(EntityId player, Func<ICustomDialogFactory, IDialog<TResponse>> dialogCreator, Action<TResponse> responseHandler)
	where TResponse : struct;
	Task<TResponse> ShowAsync<TResponse>(EntityId player, IDialog<TResponse> dialog)
	where TResponse : struct;
	void Show<TResponse>(EntityId player, IDialog<TResponse> dialog, Action<TResponse> responseHandler)
	where TResponse : struct;
}

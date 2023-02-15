using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public interface ICustomDialogService
{
	Task<MessageDialogResponse> ShowMessageAsync(Player player, Action<IMessageDialogBuilder> buildDialog);
	Task<InputDialogResponse> ShowInputAsync(Player player, Action<IInputDialogBuilder> buildDialog);
	Task<ListDialogResponse> ShowListAsync(Player player, Action<IListDialogBuilder> buildDialog);
	Task<TablistDialogResponse> ShowTablistAsync(Player player, Action<ITablistDialogBuilder> buildDialog);
	Task<TResponse> ShowAsync<TResponse>(EntityId player, IDialog<TResponse> dialog)
	where TResponse : struct;
}

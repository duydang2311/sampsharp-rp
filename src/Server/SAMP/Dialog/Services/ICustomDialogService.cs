using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public interface ICustomDialogService
{
	Task<MessageDialogResponse> ShowMessageAsync(Player player, Action<IMessageDialogBuilder> buildDialog);
	Task<InputDialogResponse> ShowInputAsync(Player player, Action<IInputDialogBuilder> buildDialog);
	Task<ListDialogResponse> ShowListAsync(Player player, Action<IListDialogBuilder> buildDialog);
	Task<TablistDialogResponse> ShowTablistAsync(Player player, Action<ITablistDialogBuilder> buildDialog);
	void ShowMessage(Player player, Action<IMessageDialogBuilder> buildDialog, Action<MessageDialogResponse> responseHandler);
	void ShowInput(Player player, Action<IInputDialogBuilder> buildDialog, Action<InputDialogResponse> responseHandler);
	void ShowList(Player player, Action<IListDialogBuilder> buildDialog, Action<ListDialogResponse> responseHandler);
	void ShowTablist(Player player, Action<ITablistDialogBuilder> buildDialog, Action<TablistDialogResponse> responseHandler);
}

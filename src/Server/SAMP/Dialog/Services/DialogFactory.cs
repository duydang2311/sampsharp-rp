using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public sealed class DialogFactory : IDialogFactory
{
	public DialogFactory() { }

	public ListDialog CreateList(Action<ListDialog> dialogAction)
	{
		var dialog = new ListDialog(string.Empty, string.Empty);
		dialogAction(dialog);
		return dialog;
	}

	public InputDialog CreateInput(Action<InputDialog> dialogAction)
	{
		var dialog = new InputDialog();
		dialogAction(dialog);
		return dialog;
	}

	public TablistDialog CreateTabList(Action<TablistDialog> dialogAction)
	{
		var dialog = new TablistDialog(string.Empty, string.Empty, string.Empty, 0);
		dialogAction(dialog);
		return dialog;
	}

	public MessageDialog CreateTabList(Action<MessageDialog> dialogAction)
	{
		var dialog = new MessageDialog(string.Empty, string.Empty, string.Empty);
		dialogAction(dialog);
		return dialog;
	}
}

using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public interface ICustomDialogFactory
{
	ListDialog CreateList(Action<ListDialog> dialogAction);
	InputDialog CreateInput(Action<InputDialog> dialogAction);
	TablistDialog CreateTabList(Action<TablistDialog> dialogAction);
	MessageDialog CreateTabList(Action<MessageDialog> dialogAction);
}

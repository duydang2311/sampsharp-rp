namespace Server.SAMP.Dialog.Services;

public interface IDialogBuilderFactory
{
	IMessageDialogBuilder CreateMessageBuilder();
	IInputDialogBuilder CreateInputBuilder();
	IListDialogBuilder CreateListBuilder();
	ITablistDialogBuilder CreateTablistBuilder();
}

namespace Server.SAMP.Dialog.Services;

public sealed class DialogBuilderFactory : IDialogBuilderFactory
{
	public IInputDialogBuilder CreateInputBuilder()
	{
		return new InputDialogBuilder();
	}

	public IListDialogBuilder CreateListBuilder()
	{
		return new ListDialogBuilder();
	}

	public IMessageDialogBuilder CreateMessageBuilder()
	{
		return new MessageDialogBuilder();
	}

	public ITablistDialogBuilder CreateTablistBuilder()
	{
		return new TablistDialogBuilder();
	}
}

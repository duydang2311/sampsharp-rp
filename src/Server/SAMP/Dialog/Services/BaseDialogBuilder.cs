namespace Server.SAMP.Dialog.Services;

public abstract class BaseDialogBuilder : IDialogBuilder
{
	protected string caption = string.Empty;
	protected string button1 = string.Empty;
	protected string? button2 = null;

	public IDialogBuilder SetCaption(string text)
	{
		caption = text;
		return this;
	}

	public IDialogBuilder SetButton1(string text)
	{
		button1 = text;
		return this;
	}

	public IDialogBuilder SetButton2(string text)
	{
		button2 = text;
		return this;
	}
}

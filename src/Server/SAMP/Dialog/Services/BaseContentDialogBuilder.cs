namespace Server.SAMP.Dialog.Services;

public abstract class BaseContentDialogBuilder : BaseDialogBuilder, IContentDialogBuilder
{
	protected string content = string.Empty;

	public IContentDialogBuilder SetContent(string text)
	{
		content = text;
		return this;
	}

	public new IContentDialogBuilder SetCaption(string text)
	{
		base.SetCaption(text);
		return this;
	}

	public new IContentDialogBuilder SetButton1(string text)
	{
		base.SetButton1(text);
		return this;
	}

	public IContentDialogBuilder SetButton2(string text)
	{
		base.SetButton2(text);
		return this;
	}
}

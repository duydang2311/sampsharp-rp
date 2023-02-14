using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public class MessageDialogBuilder : BaseDialogBuilder, IMessageDialogBuilder
{
	public new IMessageDialogBuilder SetCaption(string text)
	{
		base.SetCaption(text);
		return this;
	}

	public new IMessageDialogBuilder SetContent(string text)
	{
		base.SetContent(text);
		return this;
	}

	public new IMessageDialogBuilder SetButton1(string text)
	{
		base.SetButton1(text);
		return this;
	}

	public new IMessageDialogBuilder SetButton2(string text)
	{
		base.SetButton2(text);
		return this;
	}

	public MessageDialog Build()
	{
		return new MessageDialog(caption, content, button1, button2);
	}
}

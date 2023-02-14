using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public class InputDialogBuilder : BaseContentDialogBuilder, IInputDialogBuilder
{
	protected bool isPassword = false;

	public new IInputDialogBuilder SetCaption(string text)
	{
		base.SetCaption(text);
		return this;
	}

	public new IInputDialogBuilder SetContent(string text)
	{
		base.SetContent(text);
		return this;
	}

	public new IInputDialogBuilder SetButton1(string text)
	{
		base.SetButton1(text);
		return this;
	}

	public new IInputDialogBuilder SetButton2(string text)
	{
		base.SetButton2(text);
		return this;
	}

	public IInputDialogBuilder SetIsPassword(bool isPassword)
	{
		this.isPassword = isPassword;
		return this;
	}

	public InputDialog Build()
	{
		return new InputDialog()
		{
			Caption = caption,
			Content = content,
			Button1 = button1,
			Button2 = button2,
			IsPassword = isPassword
		};
	}
}

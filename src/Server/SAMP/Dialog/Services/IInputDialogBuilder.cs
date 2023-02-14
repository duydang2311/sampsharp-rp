using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public interface IInputDialogBuilder : IDialogBuilder
{
	new IInputDialogBuilder SetCaption(string text);
	new IInputDialogBuilder SetContent(string text);
	new IInputDialogBuilder SetButton1(string text);
	new IInputDialogBuilder SetButton2(string text);
	IInputDialogBuilder SetIsPassword(bool isPassword);
	InputDialog Build();
}

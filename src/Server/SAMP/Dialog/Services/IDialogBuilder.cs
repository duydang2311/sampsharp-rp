namespace Server.SAMP.Dialog.Services;

public interface IDialogBuilder
{
	IDialogBuilder SetCaption(string text);
	IDialogBuilder SetButton1(string text);
	IDialogBuilder SetButton2(string text);
}

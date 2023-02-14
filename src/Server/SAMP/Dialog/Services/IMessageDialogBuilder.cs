using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public interface IMessageDialogBuilder : IContentDialogBuilder
{
	new IMessageDialogBuilder SetCaption(string text);
	new IMessageDialogBuilder SetContent(string text);
	new IMessageDialogBuilder SetButton1(string text);
	new IMessageDialogBuilder SetButton2(string text);
	MessageDialog Build();
}

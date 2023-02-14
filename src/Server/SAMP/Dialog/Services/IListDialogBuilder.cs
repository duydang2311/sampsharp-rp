using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public interface IListDialogBuilder : IDialogBuilder
{
	new IListDialogBuilder SetCaption(string text);
	new IListDialogBuilder SetButton1(string text);
	new IListDialogBuilder SetButton2(string text);
	IListDialogBuilder AddRow(string text, object? tag = null);
	ListDialog Build();
}

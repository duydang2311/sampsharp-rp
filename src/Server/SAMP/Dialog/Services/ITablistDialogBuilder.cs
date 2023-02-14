using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public interface ITablistDialogBuilder : IDialogBuilder
{
	new ITablistDialogBuilder SetCaption(string text);
	new ITablistDialogBuilder SetContent(string text);
	new ITablistDialogBuilder SetButton1(string text);
	new ITablistDialogBuilder SetButton2(string text);
	ITablistDialogBuilder AddColumn(string column);
	ITablistDialogBuilder AddRow(params string[] columns);
	ITablistDialogBuilder AddRow(object tag, params string[] columns);
	TablistDialog Build();
}

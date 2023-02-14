using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public class ListDialogBuilder : BaseDialogBuilder, IListDialogBuilder
{
	protected readonly ListDialogRowCollection rows = new();

	public new IListDialogBuilder SetCaption(string text)
	{
		base.SetCaption(text);
		return this;
	}

	public new IListDialogBuilder SetContent(string text)
	{
		return this;
	}

	public new IListDialogBuilder SetButton1(string text)
	{
		base.SetButton1(text);
		return this;
	}

	public new IListDialogBuilder SetButton2(string text)
	{
		base.SetButton2(text);
		return this;
	}

	public IListDialogBuilder AddRow(string text, object? tag = null)
	{
		rows.Add(new ListDialogRow(text) { Tag = tag });
		return this;
	}

	public ListDialog Build()
	{
		var dialog = new ListDialog(caption, button1, button2);
		foreach (var row in rows)
		{
			dialog.Add(row);
		}
		return dialog;
	}
}

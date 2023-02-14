using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public class TablistDialogBuilder : BaseDialogBuilder, ITablistDialogBuilder
{
	protected readonly LinkedList<string> columns = new();
	protected readonly LinkedList<TablistDialogRow> rows = new();

	public new ITablistDialogBuilder SetCaption(string text)
	{
		base.SetCaption(text);
		return this;
	}

	public new ITablistDialogBuilder SetButton1(string text)
	{
		base.SetButton1(text);
		return this;
	}

	public new ITablistDialogBuilder SetButton2(string text)
	{
		base.SetButton2(text);
		return this;
	}

	public ITablistDialogBuilder AddColumn(string text)
	{
		columns.AddLast(text);
		return this;
	}

	public ITablistDialogBuilder AddRow(params string[] columns)
	{
		rows.AddLast(new TablistDialogRow(columns));
		return this;
	}

	public ITablistDialogBuilder AddRow(object tag, params string[] columns)
	{
		rows.AddLast(new TablistDialogRow(columns) { Tag = tag });
		return this;
	}

	public TablistDialog Build()
	{
		var dialog = new TablistDialog(caption, button1, button2, columns.ToArray());
		foreach (var row in rows)
		{
			dialog.Add(row);
		}
		return dialog;
	}
}

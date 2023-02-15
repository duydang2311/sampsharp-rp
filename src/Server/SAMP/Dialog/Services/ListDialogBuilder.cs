using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;
using Server.SAMP.Dialog.Models;

namespace Server.SAMP.Dialog.Services;

public sealed class ListDialogBuilder : BaseDialogBuilder, IListDialogBuilder
{
	private readonly LinkedList<object> rows = new();

	public ListDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService) : base(localizerService, identifierService) { }

	public override IListDialogBuilder SetCaption(string text)
	{
		base.SetCaption(text);
		return this;
	}

	public override IListDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetCaption(textIdentifier, args);
		return this;
	}

	public override IListDialogBuilder SetButton1(string text)
	{
		base.SetButton1(text);
		return this;
	}

	public override IListDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetButton1(textIdentifier, args);
		return this;
	}

	public override IListDialogBuilder SetButton2(string text)
	{
		base.SetButton2(text);
		return this;
	}

	public override IListDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetButton2(textIdentifier, args);
		return this;
	}

	public IListDialogBuilder AddRow(string text, object? tag = null)
	{
		rows.AddLast(new TaggedDialogTextModel() { Text = text, Tag = tag });
		return this;
	}

	public IListDialogBuilder AddRow(string text)
	{
		rows.AddLast(new ListDialogRow(text));
		return this;
	}

	public IListDialogBuilder AddRow(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		rows.AddLast(new TaggedDialogTextModel() { Text = identifierService.Identify(textIdentifier), Args = args });
		return this;
	}

	public IListDialogBuilder WithTag(object tag)
	{
		var last = rows.Last?.Value;
		if (last is null)
		{
			return this;
		}
		if (last is TaggedDialogTextModel model)
		{
			model.Tag = tag;
		}
		else if (last is ListDialogRow row)
		{
			row.Tag = tag;
		}
		return this;
	}

	public ListDialog Build(CultureInfo cultureInfo)
	{
		var dialog = new ListDialog(
			BuildText(cultureInfo, Caption),
			BuildText(cultureInfo, Button1),
			BuildText(cultureInfo, Button2)
		);
		foreach (var row in rows)
		{
			if (row is TaggedDialogTextModel model)
			{
				dialog.Add(new ListDialogRow(BuildText(cultureInfo, model)) { Tag = model.Tag });
			}
			else
			{
				dialog.Add((ListDialogRow)row);
			}
		}
		return dialog;
	}

	public ListDialog Build()
	{
		return Build(CultureInfo.InvariantCulture);
	}
}

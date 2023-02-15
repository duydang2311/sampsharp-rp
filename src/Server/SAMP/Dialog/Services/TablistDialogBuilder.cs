using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;
using Server.SAMP.Dialog.Models;

namespace Server.SAMP.Dialog.Services;

public sealed class TablistDialogBuilder : BaseDialogBuilder, ITablistDialogBuilder
{
	private readonly ILocalizedTextBuilderFactory textBuilderFactory;
	private readonly LinkedList<object> columns = new();
	private readonly LinkedList<object> rows = new();

	private class TablistRowBuilder
	{
		public ILocalizedTextBuilder Instance { get; set; } = null!;
		public object? Tag { get; set; }
	}

	public TablistDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService, ILocalizedTextBuilderFactory textBuilderFactory) : base(localizerService, identifierService)
	{
		this.textBuilderFactory = textBuilderFactory;
	}

	public override ITablistDialogBuilder SetCaption(string text)
	{
		base.SetCaption(text);
		return this;
	}

	public override ITablistDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetCaption(textIdentifier, args);
		return this;
	}

	public override ITablistDialogBuilder SetButton1(string text)
	{
		base.SetButton1(text);
		return this;
	}

	public override ITablistDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetButton1(textIdentifier, args);
		return this;
	}

	public override ITablistDialogBuilder SetButton2(string text)
	{
		base.SetButton2(text);
		return this;
	}

	public override ITablistDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetButton2(textIdentifier, args);
		return this;
	}

	public ITablistDialogBuilder AddColumn(string text)
	{
		columns.AddLast(text);
		return this;
	}

	public ITablistDialogBuilder AddColumn(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		columns.AddLast(new DialogTextModel
		{
			Text = identifierService.Identify(textIdentifier),
			Args = args
		});
		return this;
	}

	public ITablistDialogBuilder AddRow(params string[] columns)
	{
		rows.AddLast(new TablistDialogRow(columns));
		return this;
	}

	public ITablistDialogBuilder AddRow(Action<ILocalizedTextBuilder> buildRowTexts)
	{
		var builder = new TablistRowBuilder { Instance = textBuilderFactory.CreateBuilder() };
		buildRowTexts(builder.Instance);
		rows.AddLast(builder);
		return this;
	}

	public ITablistDialogBuilder WithTag(object tag)
	{
		var last = rows.Last?.Value;
		if (last is null)
		{
			return this;
		}
		if (last is TablistDialogRow row)
		{
			row.Tag = tag;
		}
		else if (last is TablistRowBuilder builder)
		{
			builder.Tag = tag;
		}
		return this;
	}

	public TablistDialog Build(CultureInfo cultureInfo)
	{
		var columnTexts = columns.Select(o =>
		{
			if (o is DialogTextModel model)
			{
				return BuildText(cultureInfo, model);
			}
			return (string)o;
		});
		var dialog = new TablistDialog(
			BuildText(cultureInfo, Caption),
			BuildText(cultureInfo, Button1),
			BuildText(cultureInfo, Button2),
			columnTexts.ToArray()
		);
		foreach (var obj in rows)
		{
			if (obj is TablistDialogRow row)
			{
				dialog.Add(row);
			}
			else if (obj is TablistRowBuilder builder)
			{
				dialog.Add(new TablistDialogRow(builder.Instance.Build(cultureInfo).ToArray()) { Tag = builder.Tag });
			}
		}
		return dialog;
	}

	public TablistDialog Build()
	{
		return Build(CultureInfo.InvariantCulture);
	}
}

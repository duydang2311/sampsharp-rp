using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;
using Server.SAMP.Dialog.Models;

namespace Server.SAMP.Dialog.Services;

public sealed class TablistDialogBuilder : BaseDialogBuilder<TablistDialog, ITablistDialogBuilder>, ITablistDialogBuilder
{
	private readonly IDialogTextBuilderFactory textBuilderFactory;
	private readonly LinkedList<object> columns = new();
	private readonly LinkedList<object> rows = new();

	private class TablistRowBuilder
	{
		public IDialogTextBuilder Instance { get; set; } = null!;
		public object? Tag { get; set; }
	}

	public TablistDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService, IDialogTextBuilderFactory textBuilderFactory) : base(localizerService, identifierService)
	{
		this.textBuilderFactory = textBuilderFactory;
	}
	public ITablistDialogBuilder AddColumn(string text)
	{
		columns.AddLast(text);
		return _this;
	}

	public ITablistDialogBuilder AddColumn(Expression<Func<ILocalizedDialogText, object>> textIdentifier, params object[] args)
	{
		columns.AddLast(new LocalizedTextModel
		{
			Text = identifierService.Identify(textIdentifier),
			Args = args
		});
		return _this;
	}

	public ITablistDialogBuilder AddRow(params string[] columns)
	{
		rows.AddLast(new TablistDialogRow(columns));
		return _this;
	}

	public ITablistDialogBuilder AddRow(Action<IDialogTextBuilder> buildRowTexts)
	{
		var builder = new TablistRowBuilder { Instance = textBuilderFactory.CreateBuilder() };
		buildRowTexts(builder.Instance);
		rows.AddLast(builder);
		return _this;
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
		return _this;
	}

	public override TablistDialog Build(CultureInfo cultureInfo)
	{
		var columnTexts = columns.Select(o =>
		{
			if (o is LocalizedTextModel model)
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

	public override TablistDialog Build()
	{
		return Build(CultureInfo.InvariantCulture);
	}
}

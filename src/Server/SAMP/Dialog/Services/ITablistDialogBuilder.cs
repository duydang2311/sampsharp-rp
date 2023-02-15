using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public interface ITablistDialogBuilder : IDialogBuilder
{
	new ITablistDialogBuilder SetCaption(string text);
	new ITablistDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new ITablistDialogBuilder SetButton1(string text);
	new ITablistDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new ITablistDialogBuilder SetButton2(string text);
	new ITablistDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	ITablistDialogBuilder AddColumn(string text);
	ITablistDialogBuilder AddColumn(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	ITablistDialogBuilder AddRow(params string[] columns);
	ITablistDialogBuilder AddRow(Action<ILocalizedTextBuilder> buildRowTexts);
	ITablistDialogBuilder WithTag(object tag);
	TablistDialog Build(CultureInfo cultureInfo);
	TablistDialog Build();
}

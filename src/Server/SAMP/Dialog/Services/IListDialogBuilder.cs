using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;

namespace Server.SAMP.Dialog.Services;

public interface IListDialogBuilder : IDialogBuilder
{
	new IListDialogBuilder SetCaption(string text);
	new IListDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new IListDialogBuilder SetButton1(string text);
	new IListDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new IListDialogBuilder SetButton2(string text);
	new IListDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	IListDialogBuilder AddRow(string text);
	IListDialogBuilder AddRow(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	IListDialogBuilder WithTag(object tag);
	ListDialog Build(CultureInfo cultureInfo);
	ListDialog Build();
}

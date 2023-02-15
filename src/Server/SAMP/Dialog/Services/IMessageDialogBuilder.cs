using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;

namespace Server.SAMP.Dialog.Services;

public interface IMessageDialogBuilder : IContentDialogBuilder
{
	new IMessageDialogBuilder SetCaption(string text);
	new IMessageDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new IMessageDialogBuilder SetContent(string text);
	new IMessageDialogBuilder SetContent(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new IMessageDialogBuilder SetButton1(string text);
	new IMessageDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new IMessageDialogBuilder SetButton2(string text);
	new IMessageDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	MessageDialog Build(CultureInfo cultureInfo);
	MessageDialog Build();
}

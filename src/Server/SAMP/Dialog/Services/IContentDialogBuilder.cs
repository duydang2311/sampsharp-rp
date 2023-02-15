using System.Linq.Expressions;
using Server.I18N.Localization.Models;

namespace Server.SAMP.Dialog.Services;

public interface IContentDialogBuilder : IDialogBuilder
{
	new IContentDialogBuilder SetCaption(string text);
	new IContentDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new IContentDialogBuilder SetButton1(string text);
	new IContentDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new IContentDialogBuilder SetButton2(string text);
	new IContentDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	IContentDialogBuilder SetContent(string text);
	IContentDialogBuilder SetContent(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
}

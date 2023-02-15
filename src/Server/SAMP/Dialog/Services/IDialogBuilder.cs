using System.Linq.Expressions;
using Server.I18N.Localization.Models;

namespace Server.SAMP.Dialog.Services;

// https://github.com/dotnet/csharplang/issues/49
public interface IDialogBuilder
{
	IDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdenfitier, params object[] args);
	IDialogBuilder SetCaption(string text);
	IDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdenfitier, params object[] args);
	IDialogBuilder SetButton1(string text);
	IDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdenfitier, params object[] args);
	IDialogBuilder SetButton2(string text);
}

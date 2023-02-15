using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;

namespace Server.SAMP.Dialog.Services;

// https://github.com/dotnet/csharplang/issues/49
public interface IDialogBuilder<TDialog, TBuilder>
	where TDialog : IDialog
	where TBuilder : IDialogBuilder<TDialog, TBuilder>
{
	TBuilder SetCaption(Expression<Func<ILocalizedDialogText, object>> textIdenfitier, params object[] args);
	TBuilder SetCaption(string text);
	TBuilder SetButton1(Expression<Func<ILocalizedDialogText, object>> textIdenfitier, params object[] args);
	TBuilder SetButton1(string text);
	TBuilder SetButton2(Expression<Func<ILocalizedDialogText, object>> textIdenfitier, params object[] args);
	TBuilder SetButton2(string text);
	TDialog Build(CultureInfo cultureInfo);
	TDialog Build();
}

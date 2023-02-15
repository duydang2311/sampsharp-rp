using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;

namespace Server.SAMP.Dialog.Services;

public interface IInputDialogBuilder : IContentDialogBuilder
{
	new IInputDialogBuilder SetCaption(string text);
	new IInputDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new IInputDialogBuilder SetContent(string text);
	new IInputDialogBuilder SetContent(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new IInputDialogBuilder SetButton1(string text);
	new IInputDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	new IInputDialogBuilder SetButton2(string text);
	new IInputDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	IInputDialogBuilder SetIsPassword(bool isPassword);
	InputDialog Build(CultureInfo cultureInfo);
	InputDialog Build();
}

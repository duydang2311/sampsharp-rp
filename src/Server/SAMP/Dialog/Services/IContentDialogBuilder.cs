using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;

namespace Server.SAMP.Dialog.Services;

public interface IContentDialogBuilder<TDialog, TBuilder> : IDialogBuilder<TDialog, TBuilder>
	where TDialog : IDialog
	where TBuilder : IContentDialogBuilder<TDialog, TBuilder>
{
	TBuilder SetContent(string text);
	TBuilder SetContent(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
}

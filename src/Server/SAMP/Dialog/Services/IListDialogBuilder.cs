using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;

namespace Server.SAMP.Dialog.Services;

public interface IListDialogBuilder : IDialogBuilder<ListDialog, IListDialogBuilder>
{
	IListDialogBuilder AddRow(string text);
	IListDialogBuilder AddRow(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	IListDialogBuilder WithTag(object tag);
}

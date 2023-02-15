using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public interface ITablistDialogBuilder : IDialogBuilder<TablistDialog, ITablistDialogBuilder>
{
	ITablistDialogBuilder AddColumn(string text);
	ITablistDialogBuilder AddColumn(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
	ITablistDialogBuilder AddRow(params string[] columns);
	ITablistDialogBuilder AddRow(Action<ILocalizedTextBuilder> buildRowTexts);
	ITablistDialogBuilder WithTag(object tag);
}

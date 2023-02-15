using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;

namespace Server.SAMP.Dialog.Services;

public interface ITablistDialogBuilder : IDialogBuilder<TablistDialog, ITablistDialogBuilder>
{
	ITablistDialogBuilder AddColumn(string text);
	ITablistDialogBuilder AddColumn(Expression<Func<ILocalizedDialogText, object>> textIdentifier, params object[] args);
	ITablistDialogBuilder AddRow(params string[] columns);
	ITablistDialogBuilder AddRow(Action<IDialogTextBuilder> buildRowTexts);
	ITablistDialogBuilder WithTag(object tag);
}

using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public abstract class BaseContentDialogBuilder<TDialog, TBuilder> : BaseDialogBuilder<TDialog, TBuilder>, IContentDialogBuilder<TDialog, TBuilder>
	where TDialog : IDialog
	where TBuilder : IContentDialogBuilder<TDialog, TBuilder>
{
	protected object? Content { get; set; }

	public BaseContentDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService) : base(localizerService, identifierService) { }

	public TBuilder SetContent(string text)
	{
		Content = text;
		return _this;
	}

	public TBuilder SetContent(Expression<Func<ILocalizedDialogText, object>> textIdentifier, params object[] args)
	{
		Content = new LocalizedTextModel
		{
			Text = identifierService.Identify(textIdentifier),
			Args = args
		};
		return _this;
	}
}

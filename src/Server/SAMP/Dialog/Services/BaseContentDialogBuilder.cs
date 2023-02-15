using System.Linq.Expressions;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;
using Server.SAMP.Dialog.Models;

namespace Server.SAMP.Dialog.Services;

public abstract class BaseContentDialogBuilder : BaseDialogBuilder, IContentDialogBuilder
{
	protected object? Content { get; set; }

	public BaseContentDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService) : base(localizerService, identifierService) { }

	public virtual IContentDialogBuilder SetContent(string text)
	{
		Content = text;
		return this;
	}

	public virtual IContentDialogBuilder SetContent(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		Content = new DialogTextModel
		{
			Text = identifierService.Identify(textIdentifier),
			Args = args
		};
		return this;
	}

	public override IContentDialogBuilder SetCaption(string text)
	{
		base.SetCaption(text);
		return this;
	}

	public override IContentDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetCaption(textIdentifier, args);
		return this;
	}

	public override IContentDialogBuilder SetButton1(string text)
	{
		base.SetButton1(text);
		return this;
	}

	public override IContentDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetButton1(textIdentifier, args);
		return this;
	}

	public override IContentDialogBuilder SetButton2(string text)
	{
		base.SetButton2(text);
		return this;
	}

	public override IContentDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetButton2(textIdentifier, args);
		return this;
	}
}

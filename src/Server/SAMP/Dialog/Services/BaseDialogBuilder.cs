using System.Globalization;
using System.Linq.Expressions;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;
using Server.SAMP.Dialog.Models;

namespace Server.SAMP.Dialog.Services;

public abstract class BaseDialogBuilder : IDialogBuilder
{
	protected readonly ITextLocalizerService localizerService;
	protected readonly ITextNameIdentifierService identifierService;

	protected object? Caption { get; set; }
	protected object? Button1 { get; set; }
	protected object? Button2 { get; set; }

	public BaseDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService)
	{
		this.localizerService = localizerService;
		this.identifierService = identifierService;
	}

	public virtual IDialogBuilder SetCaption(string text)
	{
		Caption = text;
		return this;
	}

	public virtual IDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		Caption = new DialogTextModel()
		{
			Text = identifierService.Identify(textIdentifier),
			Args = args
		};
		return this;
	}

	public virtual IDialogBuilder SetButton1(string text)
	{
		Button1 = text;
		return this;
	}

	public virtual IDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		Button1 = new DialogTextModel()
		{
			Text = identifierService.Identify(textIdentifier),
			Args = args
		};
		return this;
	}

	public virtual IDialogBuilder SetButton2(string text)
	{
		Button2 = text;
		return this;
	}

	public virtual IDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		Button2 = new DialogTextModel()
		{
			Text = identifierService.Identify(textIdentifier),
			Args = args
		};
		return this;
	}

	protected string BuildText(CultureInfo cultureInfo, object? text)
	{
		if (text is DialogTextModel model)
		{
			return localizerService.Get(cultureInfo, model.Text, model.Args);
		}
		return text is null ? string.Empty : (string)text;
	}
}

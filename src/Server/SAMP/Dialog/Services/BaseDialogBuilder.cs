using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;
using Server.SAMP.Dialog.Models;

namespace Server.SAMP.Dialog.Services;

public abstract class BaseDialogBuilder<TDialog, TBuilder> : IDialogBuilder<TDialog, TBuilder>
	where TDialog : IDialog
	where TBuilder : IDialogBuilder<TDialog, TBuilder>
{
	protected readonly TBuilder _this;
	protected readonly ITextLocalizerService localizerService;
	protected readonly ITextNameIdentifierService identifierService;

	protected object? Caption { get; set; }
	protected object? Button1 { get; set; }
	protected object? Button2 { get; set; }

	public BaseDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService)
	{
		this.localizerService = localizerService;
		this.identifierService = identifierService;
		_this = (TBuilder)(IDialogBuilder<TDialog, TBuilder>)this;
	}

	public TBuilder SetCaption(string text)
	{
		Caption = text;
		return _this;
	}

	public TBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		Caption = new DialogTextModel()
		{
			Text = identifierService.Identify(textIdentifier),
			Args = args
		};
		return _this;
	}

	public TBuilder SetButton1(string text)
	{
		Button1 = text;
		return _this;
	}

	public TBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		Button1 = new DialogTextModel()
		{
			Text = identifierService.Identify(textIdentifier),
			Args = args
		};
		return _this;
	}

	public TBuilder SetButton2(string text)
	{
		Button2 = text;
		return _this;
	}

	public TBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		Button2 = new DialogTextModel()
		{
			Text = identifierService.Identify(textIdentifier),
			Args = args
		};
		return _this;
	}

	protected string BuildText(CultureInfo cultureInfo, object? text)
	{
		if (text is DialogTextModel model)
		{
			return localizerService.Get(cultureInfo, model.Text, model.Args);
		}
		return text is null ? string.Empty : (string)text;
	}

	public abstract TDialog Build(CultureInfo cultureInfo);
	public abstract TDialog Build();
}

using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public class InputDialogBuilder : BaseContentDialogBuilder, IInputDialogBuilder
{
	protected bool isPassword = false;

	public InputDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService) : base(localizerService, identifierService) { }

	public override IInputDialogBuilder SetCaption(string text)
	{
		base.SetCaption(text);
		return this;
	}

	public override IInputDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetCaption(textIdentifier, args);
		return this;
	}

	public override IInputDialogBuilder SetContent(string text)
	{
		base.SetContent(text);
		return this;
	}

	public override IInputDialogBuilder SetContent(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetContent(textIdentifier, args);
		return this;
	}

	public override IInputDialogBuilder SetButton1(string text)
	{
		base.SetButton1(text);
		return this;
	}

	public override IInputDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetButton1(textIdentifier, args);
		return this;
	}

	public override IInputDialogBuilder SetButton2(string text)
	{
		base.SetButton2(text);
		return this;
	}

	public override IInputDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetButton2(textIdentifier, args);
		return this;
	}

	public IInputDialogBuilder SetIsPassword(bool isPassword)
	{
		this.isPassword = isPassword;
		return this;
	}

	public InputDialog Build(CultureInfo cultureInfo)
	{
		return new InputDialog()
		{
			Caption = BuildText(cultureInfo, Caption),
			Content = BuildText(cultureInfo, Content),
			Button1 = BuildText(cultureInfo, Button1),
			Button2 = BuildText(cultureInfo, Button2),
			IsPassword = isPassword
		};
	}

	public InputDialog Build()
	{
		return Build(CultureInfo.InvariantCulture);
	}
}

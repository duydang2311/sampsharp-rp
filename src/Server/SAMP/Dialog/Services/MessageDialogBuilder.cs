using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public class MessageDialogBuilder : BaseContentDialogBuilder, IMessageDialogBuilder
{
	public MessageDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService) : base(localizerService, identifierService) { }

	public override IMessageDialogBuilder SetCaption(string text)
	{
		base.SetCaption(text);
		return this;
	}

	public override IMessageDialogBuilder SetCaption(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetCaption(textIdentifier, args);
		return this;
	}

	public override IMessageDialogBuilder SetContent(string text)
	{
		base.SetContent(text);
		return this;
	}

	public override IMessageDialogBuilder SetContent(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetContent(textIdentifier, args);
		return this;
	}

	public override IMessageDialogBuilder SetButton1(string text)
	{
		base.SetButton1(text);
		return this;
	}

	public override IMessageDialogBuilder SetButton1(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetButton1(textIdentifier, args);
		return this;
	}

	public override IMessageDialogBuilder SetButton2(string text)
	{
		base.SetButton2(text);
		return this;
	}

	public override IMessageDialogBuilder SetButton2(Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		base.SetButton2(textIdentifier, args);
		return this;
	}

	public MessageDialog Build(CultureInfo cultureInfo)
	{
		return new MessageDialog(
			BuildText(cultureInfo, Caption),
			BuildText(cultureInfo, Content),
			BuildText(cultureInfo, Button1),
			BuildText(cultureInfo, Button2)
		);
	}

	public MessageDialog Build()
	{
		return Build(CultureInfo.InvariantCulture);
	}
}

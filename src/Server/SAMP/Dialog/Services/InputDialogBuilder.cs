using System.Globalization;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public class InputDialogBuilder : BaseContentDialogBuilder<InputDialog, IInputDialogBuilder>, IInputDialogBuilder
{
	protected bool isPassword = false;

	public InputDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService) : base(localizerService, identifierService) { }

	public IInputDialogBuilder SetIsPassword(bool isPassword)
	{
		this.isPassword = isPassword;
		return this;
	}

	public override InputDialog Build(CultureInfo cultureInfo)
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

	public override InputDialog Build()
	{
		return Build(CultureInfo.InvariantCulture);
	}
}

using System.Globalization;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public sealed class MessageDialogBuilder : BaseContentDialogBuilder<MessageDialog, IMessageDialogBuilder>, IMessageDialogBuilder
{
	public MessageDialogBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService) : base(localizerService, identifierService) { }

	public override MessageDialog Build(CultureInfo cultureInfo)
	{
		return new MessageDialog(
			BuildText(cultureInfo, Caption),
			BuildText(cultureInfo, Content),
			BuildText(cultureInfo, Button1),
			BuildText(cultureInfo, Button2)
		);
	}

	public override MessageDialog Build()
	{
		return Build(CultureInfo.InvariantCulture);
	}
}

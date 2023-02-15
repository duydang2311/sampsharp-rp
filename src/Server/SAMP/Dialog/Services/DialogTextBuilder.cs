using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public sealed class DialogTextBuilder : LocalizedTextBuilder<LocalizedTextModel, ILocalizedDialogText, IDialogTextBuilder>, IDialogTextBuilder
{
	public DialogTextBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService) : base(localizerService, identifierService) { }
}

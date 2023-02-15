using Server.I18N.Localization.Models;

namespace Server.SAMP.Dialog.Models;

public class TaggedDialogTextModel : LocalizedTextModel
{
	public object? Tag { get; set; }
}

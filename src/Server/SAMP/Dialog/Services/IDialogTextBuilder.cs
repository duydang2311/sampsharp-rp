using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.SAMP.Dialog.Services;

public interface IDialogTextBuilder : ILocalizedTextBuilder<LocalizedTextModel, ILocalizedDialogText, IDialogTextBuilder> { }

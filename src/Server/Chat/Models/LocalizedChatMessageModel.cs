using SampSharp.Entities.SAMP;
using Server.Common.Colors;
using Server.I18N.Localization.Models;

namespace Server.Chat.Models;

public class LocalizedChatMessageModel : LocalizedTextModel
{
	public Color Color { get; set; } = SemanticColor.Neutral;
	public bool IsInline { get; set; } = false;
}

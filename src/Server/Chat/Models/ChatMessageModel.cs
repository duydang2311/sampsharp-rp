using SampSharp.Entities.SAMP;
using Server.Common.Colors;

namespace Server.Chat.Models;

public class ChatMessageModel
{
	public string Text { get; set; } = string.Empty;
	public Color Color { get; set; } = SemanticColor.Neutral;
}

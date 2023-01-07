using SampSharp.Entities.SAMP;

namespace Server.Chat.Models;

public sealed class ChatMessageModel
{
	public string Text { get; set; } = string.Empty;
	public Color Color { get; set; } = Color.White;
}

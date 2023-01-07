using SampSharp.Entities.SAMP;
using Server.Chat.Models;

namespace Server.Chat.Services;

public sealed class ChatMessageModelFactory : IChatMessageModelFactory
{
	public ChatMessageModel Create(Color color, string text)
	{
		return new ChatMessageModel() { Color = color, Text = text };
	}
	public ChatMessageModel Create(string text)
	{
		return new ChatMessageModel() { Color = Color.White, Text = text };
	}
}

using SampSharp.Entities.SAMP;
using Server.Chat.Models;

namespace Server.Chat.Services;

public interface IChatMessageModelFactory
{
	ChatMessageModel Create(Color color, string text);
	ChatMessageModel Create(string text);
}

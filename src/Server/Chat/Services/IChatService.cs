using SampSharp.Entities.SAMP;
using Server.Chat.Models;

namespace Server.Chat.Services;

public interface IChatService
{
	void SendMessage(Player player, Func<IChatMessageModelFactory, ChatMessageModel> messageCreator);
	void SendMessages(Player player, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator);
	void SendInlineMessages(Player player, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator);
}

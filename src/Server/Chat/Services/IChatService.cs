using SampSharp.Entities.SAMP;
using Server.Chat.Models;

namespace Server.Chat.Services;

public interface IChatService
{
	void SendMessage(Player player, Func<IChatMessageModelFactory, ChatMessageModel> messageCreator);
	void SendMessages(Player player, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator);
	void SendInlineMessages(Player player, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator);void        void SendMessage(Predicate<Player> filter, Func<IChatMessageModelFactory, ChatMessageModel> messageCreator);
    void SendMessages(Predicate<Player> filter, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator);
    void SendInlineMessages(Predicate<Player> filter, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator);
}

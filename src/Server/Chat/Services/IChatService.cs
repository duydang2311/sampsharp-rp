using System.Linq.Expressions;
using SampSharp.Entities.SAMP;

namespace Server.Chat.Services;

public interface IChatService
{
	void SendMessage(Player player, Action<IChatMessageBuilder> buildActions);
	void SendMessage(Predicate<Player> filter, Action<IChatMessageBuilder> buildActions);
}

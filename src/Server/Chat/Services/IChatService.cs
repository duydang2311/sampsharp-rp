using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.Chat.Models;
using Server.I18n.Localization.Models;

namespace Server.Chat.Services;

public interface IChatService
{
	void SendMessage(Player player, Expression<Func<ITextNameFakeModel, object>> textIdentifier);
	void SendMessage(Player player, Color color, Expression<Func<ITextNameFakeModel, object>> textIdentifier);
	void SendMessage(Player player, Expression<Func<ITextNameFakeModel, object>> textIdentifier, params object[] args);
	void SendMessage(Player player, Color color, Expression<Func<ITextNameFakeModel, object>> textIdentifier, params object[] args);
	void SendMessage(Player player, Func<IChatMessageModelFactory, ChatMessageModel> messageCreator);
	void SendMessages(Player player, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator);
	void SendInlineMessages(Player player, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator);
}

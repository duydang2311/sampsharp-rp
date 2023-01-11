using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.Chat.Models;
using Server.I18n.Localization.Models;

namespace Server.Chat.Services;

public interface IChatMessageModelFactory
{
	ChatMessageModel Create(Expression<Func<ITextNameFakeModel, object>> identifier);
	ChatMessageModel Create(Color color, Expression<Func<ITextNameFakeModel, object>> identifier);
	ChatMessageModel Create(Expression<Func<ITextNameFakeModel, object>> identifier, params object[] args);
	ChatMessageModel Create(Color color, Expression<Func<ITextNameFakeModel, object>> identifier, params object[] args);
	ChatMessageModel Create(string text);
	ChatMessageModel Create(Color color, string text);
}

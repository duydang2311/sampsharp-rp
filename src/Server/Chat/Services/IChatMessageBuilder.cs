using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.Chat.Models;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.Chat.Services;

public interface IChatMessageBuilder : ILocalizedTextBuilder<LocalizedChatMessageModel, ILocalizedText, IChatMessageBuilder>
{
	IChatMessageBuilder Add(Color color, Expression<Func<ILocalizedText, object>> textIdentifer, params object[] args);
	IChatMessageBuilder Add(Color color, string text, params object[] args);
	IChatMessageBuilder Inline(Expression<Func<ILocalizedText, object>> textIdentifer, params object[] args);
	IChatMessageBuilder Inline(Color color, Expression<Func<ILocalizedText, object>> textIdentifer, params object[] args);
	IChatMessageBuilder Inline(string text, params object[] args);
	IChatMessageBuilder Inline(Color color, string text, params object[] args);
}

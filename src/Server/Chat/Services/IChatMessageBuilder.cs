using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;

namespace Server.Chat.Services;

public interface IChatMessageBuilder
{
	IChatMessageBuilder Add(Expression<Func<ILocalizedText, object>> textIdentifer, params object[] args);
	IChatMessageBuilder Add(Color color, Expression<Func<ILocalizedText, object>> textIdentifer, params object[] args);
	IChatMessageBuilder Add(string text, params object[] args);
	IChatMessageBuilder Add(Color color, string text, params object[] args);
	IChatMessageBuilder Inline(Expression<Func<ILocalizedText, object>> textIdentifer, params object[] args);
	IChatMessageBuilder Inline(Color color, Expression<Func<ILocalizedText, object>> textIdentifer, params object[] args);
	IChatMessageBuilder Inline(string text, params object[] args);
	IChatMessageBuilder Inline(Color color, string text, params object[] args);
	IEnumerable<string> Build(CultureInfo cultureInfo);
	IChatMessageBuilder AddBadge(Expression<Func<ILocalizedBadge, object>> badgeIdentifier);
	IChatMessageBuilder InlineBadge(Expression<Func<ILocalizedBadge, object>> badgeIdentifier);
}

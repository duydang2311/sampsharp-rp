using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;

namespace Server.Chat.Services;

public interface IChatMessageBuilder
{
	IChatMessageBuilder Add(Expression<Func<ITextNameFakeModel, object>> textNameIdentifier, params object[] args);
	IChatMessageBuilder Add(Color color, Expression<Func<ITextNameFakeModel, object>> textNameIdentifier, params object[] args);
	IChatMessageBuilder Inline(Expression<Func<ITextNameFakeModel, object>> textNameIdentifier, params object[] args);
	IChatMessageBuilder Inline(Color color, Expression<Func<ITextNameFakeModel, object>> textNameIdentifier, params object[] args);
	IEnumerable<string> Build(CultureInfo cultureInfo);
}

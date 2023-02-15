using System.Globalization;
using System.Linq.Expressions;
using Server.I18N.Localization.Models;

namespace Server.I18N.Localization.Services;

public interface ILocalizedTextBuilder
{
	ILocalizedTextBuilder Add(Expression<Func<ILocalizedText, object>> textIdentifer, params object[] args);
	ILocalizedTextBuilder Add(string text, params object[] args);
	IEnumerable<string> Build(CultureInfo cultureInfo);
}

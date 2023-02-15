using System.Globalization;
using System.Linq.Expressions;
using Server.I18N.Localization.Models;

namespace Server.I18N.Localization.Services;

public interface ILocalizedTextBuilder<TModel, TInterface, TBuilder>
	where TModel : LocalizedTextModel
	where TInterface : class
	where TBuilder : ILocalizedTextBuilder<TModel, TInterface, TBuilder>
{
	TBuilder Add(Expression<Func<TInterface, object>> textIdentifer, params object[] args);
	TBuilder Add(string text, params object[] args);
	IEnumerable<string> Build(CultureInfo cultureInfo);
}

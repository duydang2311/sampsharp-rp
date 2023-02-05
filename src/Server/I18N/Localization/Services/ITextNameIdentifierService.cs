using System.Linq.Expressions;

namespace Server.I18N.Localization.Services;

public interface ITextNameIdentifierService
{
	string Identify<T>(Expression<Func<T, object>> identifier);
}

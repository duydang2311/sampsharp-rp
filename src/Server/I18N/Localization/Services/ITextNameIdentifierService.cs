using System.Linq.Expressions;
using Server.I18N.Localization.Models;

namespace Server.I18N.Localization.Services;

public interface ITextNameIdentifierService
{
	string Identify(Expression<Func<ITextNameFakeModel, object>> identifier);
}

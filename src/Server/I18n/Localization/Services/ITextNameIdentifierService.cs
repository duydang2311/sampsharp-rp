using System.Linq.Expressions;
using Server.I18n.Localization.Models;

namespace Server.I18n.Localization.Services;

public interface ITextNameIdentifierService
{
	string Identify(Expression<Func<ITextNameFakeModel, object>> identifier);
}

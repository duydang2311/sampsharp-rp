using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Models;

namespace Server.I18N.Localization.Services;

public interface IPlayerTextLocalizerService
{
	string Get(Player player, Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args);
}

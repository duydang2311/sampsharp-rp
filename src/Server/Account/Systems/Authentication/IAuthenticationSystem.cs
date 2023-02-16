using SampSharp.Entities;
using SampSharp.Entities.SAMP;


namespace Server.Account.Systems.Authentication;

public interface IAuthenticationSystem : ISystem
{
	Task<bool> IsAccountSignedUpAsync(Player player);
}

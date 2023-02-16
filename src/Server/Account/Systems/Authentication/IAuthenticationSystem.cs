using SampSharp.Entities;
using SampSharp.Entities.SAMP;


namespace Server.Account.Systems.Authentication;

public interface IAuthenticationSystem : ISystem
{
	Task AuthenticateAsync(Player player);
}

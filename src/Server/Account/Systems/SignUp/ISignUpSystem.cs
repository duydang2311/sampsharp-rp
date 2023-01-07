using SampSharp.Entities.SAMP;

namespace Server.Account.Systems.SignUp;

public interface ISignUpSystem
{
    Task SignUp(Player player);
}
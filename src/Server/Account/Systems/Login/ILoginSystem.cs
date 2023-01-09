using SampSharp.Entities.SAMP;

namespace Server.Account.Systems.Login;

public interface ILoginSystem
{
    void Login(Player player);
}
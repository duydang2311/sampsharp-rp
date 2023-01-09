using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Account.Systems.Login;

public interface ILoginEvent : IEvent<Player, long>
{   
    
}
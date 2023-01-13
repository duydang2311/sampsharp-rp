using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Account.Systems.Authentication;

public interface IAuthenticatedEvent : IEvent<Player, bool>
{
}
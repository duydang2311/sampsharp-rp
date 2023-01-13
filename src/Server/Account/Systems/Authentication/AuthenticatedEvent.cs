using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Account.Systems.Authentication;

public sealed class AuthenticatedEvent : BaseEvent<Player, bool>, IAuthenticatedEvent
{
}
using SampSharp.Entities.SAMP;
using Server.Common.CancellableEvent;

namespace Server.Account.Systems.Authentication;

public sealed class AuthenticatedEvent : BaseCancellableEvent<Player, bool>, IAuthenticatedEvent
{
	public AuthenticatedEvent(ICancellableEventInvoker invoker) : base(invoker) { }
}

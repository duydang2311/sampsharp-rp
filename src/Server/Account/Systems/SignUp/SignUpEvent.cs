using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Account.Systems.SignUp;

public sealed class SignedUpEvent : BaseEvent<Player>, ISignedUpEvent
{
	public SignedUpEvent(IEventInvoker invoker) : base(invoker) { }
}

using SampSharp.Entities.SAMP;
using Server.Common.CancellableEvent;

namespace Server.Character.Systems.EnterCommand;

public sealed class EnterCommandEvent : BaseCancellableEvent<Player>, IEnterCommandEvent
{
	public EnterCommandEvent(ICancellableEventInvoker invoker) : base(invoker) { }
}

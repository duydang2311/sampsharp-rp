using SampSharp.Entities.SAMP;
using Server.Common.CancellableEvent;

namespace Server.Character.Systems.ExitCommand;

public sealed class ExitCommandEvent : BaseCancellableEvent<Player>, IExitCommandEvent
{
	public ExitCommandEvent(ICancellableEventInvoker invoker) : base(invoker) { }
}

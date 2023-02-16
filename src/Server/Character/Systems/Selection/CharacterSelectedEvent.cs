using SampSharp.Entities.SAMP;
using Server.Common.CancellableEvent;

namespace Server.Character.Systems.Selection;

public sealed class CharacterSelectedEvent : BaseCancellableEvent<Player, long>, ICharacterSelectedEvent
{
	public CharacterSelectedEvent(ICancellableEventInvoker invoker) : base(invoker) { }
}

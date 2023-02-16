using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Character.Systems.Creation;

public sealed class CharacterCreatedEvent : BaseEvent<Player>, ICharacterCreatedEvent
{
	public CharacterCreatedEvent(IEventInvoker invoker) : base(invoker) { }
}

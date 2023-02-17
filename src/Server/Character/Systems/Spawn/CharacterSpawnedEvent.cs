using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Character.Systems.Spawn;

public sealed class CharacterSpawnedEvent : BaseEvent<Player>, ICharacterSpawnedEvent
{
	public CharacterSpawnedEvent(IEventInvoker invoker) : base(invoker) { }
}

using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Character.Systems.Spawn;

public interface ICharacterSpawnedEvent : IEvent<Player> { }

using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Character.Systems.Creation;

public interface ICharacterCreatedEvent : IEvent<Player> { }

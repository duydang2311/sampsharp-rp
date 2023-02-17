using SampSharp.Entities.SAMP;
using Server.Common.CancellableEvent;

namespace Server.Character.Systems.Selection;

public interface ICharacterSelectedEvent : ICancellableEvent<Player, long> { }

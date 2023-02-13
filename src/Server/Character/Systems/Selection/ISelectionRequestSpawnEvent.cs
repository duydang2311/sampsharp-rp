using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Character.Systems.Selection;

public interface ISelectionRequestSpawnEvent : IEvent<Player, long>
{

}
using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Character.Systems.Spawn;

public class SpawnRequestSelectionEvent : BaseEvent<Player>, ISpawnRequestSelectionEvent
{
    public SpawnRequestSelectionEvent(IEventInvoker invoker) : base(invoker)
    {
        
    }
}
using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Character.Systems.Selection;

public class SelectionRequestSpawnEvent : BaseEvent<Player, long>, ISelectionRequestSpawnEvent
{
    public SelectionRequestSpawnEvent(IEventInvoker invoker) : base(invoker)
    {
        
    }
}
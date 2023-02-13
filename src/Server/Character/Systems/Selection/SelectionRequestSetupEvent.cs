using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Character.Systems.Selection;

public class SelectionRequestSetupEvent : BaseEvent<Player>, ISelectionRequestSetupEvent
{
    public SelectionRequestSetupEvent(IEventInvoker invoker) : base(invoker)
    {
        
    }
}
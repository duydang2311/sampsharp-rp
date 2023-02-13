using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Character.Systems.Creation;

public sealed class CreationFinishedEvent : BaseEvent<Player>, ICreationFinishedEvent
{
    public CreationFinishedEvent(IEventInvoker invoker) : base(invoker)
    {
        
    }
}
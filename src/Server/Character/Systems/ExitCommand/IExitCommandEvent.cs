using SampSharp.Entities.SAMP;
using Server.Common.CancellableEvent;

namespace Server.Character.Systems.ExitCommand;

public interface IExitCommandEvent : ICancellableEvent<Player>
{

}

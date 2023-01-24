using SampSharp.Entities.SAMP;
using Server.Common.CancellableEvent;

namespace Server.Character.Systems.EnterCommand;

public interface IEnterCommandEvent : ICancellableEvent<Player>
{

}

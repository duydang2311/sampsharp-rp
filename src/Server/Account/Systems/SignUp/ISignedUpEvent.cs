using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Account.Systems.SignUp;

public interface ISignedUpEvent : IEvent<Player> { }

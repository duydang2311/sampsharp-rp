using SampSharp.Entities.SAMP;
using Server.Common.CancellableEvent;

namespace Server.Account.Systems.Authentication;

public interface IAuthenticatedEvent : ICancellableEvent<Player, bool> { }

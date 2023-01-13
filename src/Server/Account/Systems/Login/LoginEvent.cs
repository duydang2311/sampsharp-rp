using SampSharp.Entities.SAMP;
using Server.Common.Event;

namespace Server.Account.Systems.Login;

public sealed class LoginEvent : BaseEvent<Player>, ILoginEvent
{
}
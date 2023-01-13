using Server.Account.Systems.Authentication;
using Server.Account.Systems.Login;
using Server.Account.Systems.SignUp;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ExtendIServiceCollection
{
    public static IServiceCollection WithAccount(this IServiceCollection self)
    {
        return self
            .AddSingleton<LoginSystem>()
            .AddSingleton<SignUpSystem>()
            .AddSingleton<IAuthenticatedEvent, AuthenticatedEvent>()
            .AddSingleton<ISignedUpEvent, SignedUpEvent>()
            .AddSingleton<ILoginEvent, LoginEvent>();
    }
}

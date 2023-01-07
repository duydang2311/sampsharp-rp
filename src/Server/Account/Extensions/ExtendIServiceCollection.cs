using Server.Account.Systems.Login;
using Server.Account.Systems.SignUp;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ExtendIServiceCollection
{
    public static IServiceCollection WithAccount(this IServiceCollection self)
    {
        return self
            .AddSingleton<ILoginSystem, LoginSystem>()
            .AddSingleton<ISignUpSystem, SignUpSystem>();
    }
}

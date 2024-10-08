using SampSharp.Entities;
using Server.Account.Systems.Authentication;
using Server.Account.Systems.Login;
using Server.Account.Systems.SignUp;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithAccount(this IServiceCollection self)
	{
		return self
			.AddSystem<AuthenticationSystem>()
			.AddSystem<LoginSystem>()
			.AddSystem<SignUpSystem>()
			.AddSingleton<IAuthenticationSystem>(provider => provider.GetRequiredService<AuthenticationSystem>())
			.AddSingleton<IAuthenticatedEvent, AuthenticatedEvent>()
			.AddSingleton<ISignedUpEvent, SignedUpEvent>()
			.AddSingleton<ILoginEvent, LoginEvent>();
	}
}

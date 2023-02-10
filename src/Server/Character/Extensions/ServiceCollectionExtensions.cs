using SampSharp.Entities;
using Server.Character.Systems.EnterCommand;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithCharacter(this IServiceCollection self)
	{
		return self
			.AddSystem<EnterCommandSystem>()
			.AddSingleton<IEnterCommandEvent, EnterCommandEvent>();
	}
}

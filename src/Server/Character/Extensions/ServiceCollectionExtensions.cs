using SampSharp.Entities;
using Server.Character.Systems.EnterCommand;
using Server.Character.Systems.ExitCommand;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithCharacter(this IServiceCollection self)
	{
		return self
			.AddSystem<EnterCommandSystem>()
			.AddSystem<ExitCommandSystem>()
			.AddSingleton<IEnterCommandEvent, EnterCommandEvent>()
			.AddSingleton<IExitCommandEvent, ExitCommandEvent>();
	}
}

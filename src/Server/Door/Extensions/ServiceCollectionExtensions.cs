using SampSharp.Entities;
using Server.Door.Services;
using Server.Door.Systems.DoorCommand;
using Server.Door.Systems.Enter;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithDoor(this IServiceCollection self)
	{
		return self
			.AddSystem<DoorCommandSystem>()
			.AddSystem<EnterSystem>()
			.AddSingleton<IDoorFactory, DoorFactory>();
	}
}

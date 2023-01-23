using SampSharp.Entities;
using Server.Door.Services;
using Server.Door.Systems.DoorCommand;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class IServiceCollectionExtensions
{
	public static IServiceCollection WithDoor(this IServiceCollection self)
	{
		return self
			.AddSystem<DoorCommandSystem>()
			.AddSingleton<IDoorFactory, DoorFactory>();
	}
}

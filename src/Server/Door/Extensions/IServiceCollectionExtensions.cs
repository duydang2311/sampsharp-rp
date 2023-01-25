using SampSharp.Entities;
using Server.Door.Services;
using Server.Door.Systems.DoorCommand;
using Server.Door.Systems.Enter;
using Server.Door.Systems.Exit;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class IServiceCollectionExtensions
{
	public static IServiceCollection WithDoor(this IServiceCollection self)
	{
		return self
			.AddSystem<DoorCommandSystem>()
			.AddSystem<EnterSystem>()
			.AddSystem<ExitSystem>()
			.AddSingleton<IDoorFactory, DoorFactory>();
	}
}

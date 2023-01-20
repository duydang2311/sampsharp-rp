using Server.Door.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class IServiceCollectionExtensions
{
	public static IServiceCollection WithDoor(this IServiceCollection self)
	{
		return self
			.AddSingleton<IDoorFactory, DoorFactory>();
	}
}

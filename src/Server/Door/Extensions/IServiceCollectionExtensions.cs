using SampSharp.Entities;
using Server.Door.Systems;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class IServiceCollectionExtensions
{
	public static IServiceCollection WithDoor(this IServiceCollection self)
	{
		return self
			.AddSystem<TestSystem>();
	}
}

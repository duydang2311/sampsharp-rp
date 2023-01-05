using SampSharp.Entities;
using Server.Chat.Middlewares;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ExtendIEcsBuilder
{
	public static IEcsBuilder UseChatMiddlewares(this IEcsBuilder self)
	{
		return self.UseMiddleware<PlayerCommandMiddleware>("OnPlayerCommandText");
	}
}

using SampSharp.Entities;
using Server.Chat.Middlewares;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class IEcsBuilderExtensions
{
	public static IEcsBuilder UseChatMiddlewares(this IEcsBuilder self)
	{
		return self
            .UseMiddleware<CommandMiddleware>("OnPlayerCommandText")
            .UseMiddleware<TextMiddleware>("OnPlayerText");
	}
}

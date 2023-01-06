using Server.Chat.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ExtendIServiceCollection
{
	public static IServiceCollection WithChat(this IServiceCollection self)
	{
		return self
			.AddSingleton<ICommandService, CommandService>()
			.AddSingleton<IArgumentParser>(provider => new CommandArgumentParser(1));
	}
}

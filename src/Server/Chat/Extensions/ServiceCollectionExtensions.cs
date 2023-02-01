using Server.Chat.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithChat(this IServiceCollection self)
	{
		return self
			.AddSingleton<ICommandService, CommandService>()
			.AddSingleton<IChatService, ChatService>()
			.AddSingleton<IChatMessageModelFactory, ChatMessageModelFactory>()
			.AddSingleton<IArgumentParser>(_ => new CommandArgumentParser(1))
			.AddSingleton<ICommandModelFactory, CommandModelFactory>()
			.AddSingleton<IChatMessageBuilderFactory, ChatMessageBuilderFactory>();
	}
}

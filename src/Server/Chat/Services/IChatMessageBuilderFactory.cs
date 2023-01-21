namespace Server.Chat.Services;

public interface IChatMessageBuilderFactory
{
	IChatMessageBuilder CreateBuilder();
}

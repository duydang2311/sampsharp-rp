using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Components;

namespace Server.Chat.Services;

public sealed class ChatService : IChatService
{
	private readonly IEntityManager entityManager;
	private readonly IChatMessageBuilderFactory builderFactory;

	public ChatService(IEntityManager entityManager, IChatMessageBuilderFactory builderFactory)
	{
		this.entityManager = entityManager;
		this.builderFactory = builderFactory;
	}

	private static void SendMessage(Player player, Color color, string text)
	{
		player.SendClientMessage(color, text);
	}

	private static IEnumerable<string> BuildChatMessageBuilder(Player player, IChatMessageBuilder builder)
	{
		return builder.Build(player.GetComponent<CultureComponent>().Culture);
	}

	public void SendMessage(Player player, Action<IChatMessageBuilder> buildActions)
	{
		var builder = builderFactory.CreateBuilder();
		buildActions(builder);
		foreach(var text in BuildChatMessageBuilder(player, builder))
		{
			SendMessage(player, Color.White, text);
		}
	}

	public void SendMessage(Predicate<Player> filter, Action<IChatMessageBuilder> buildActions)
	{
		var builder = builderFactory.CreateBuilder();
		buildActions(builder);
		foreach (var p in entityManager.GetComponents<Player>())
		{
			if (filter(p))
			{
				foreach(var text in BuildChatMessageBuilder(p, builder))
				{
					SendMessage(p, Color.White, text);
				}
			}
		}
	}
}

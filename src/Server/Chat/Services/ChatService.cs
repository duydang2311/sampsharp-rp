using System.Text;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Models;

namespace Server.Chat.Services;

public sealed class ChatService : IChatService
{
	private readonly IChatMessageModelFactory factory;
    private readonly IEntityManager entityManager;

	public ChatService(IChatMessageModelFactory factory, IEntityManager entityManager)
	{
		this.factory = factory;
        this.entityManager = entityManager;
	}

	private static void SendMessage(Player player, Color color, string message)
	{
		player.SendClientMessage(color, message);
	}

	public void SendMessage(Player player, Func<IChatMessageModelFactory, ChatMessageModel> messageCreator)
	{
		var model = messageCreator(factory);
		SendMessage(player, model.Color, model.Text);
	}

	public void SendMessages(Player player, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator)
	{
		var models = messageCreator(factory);
		foreach (var model in models)
		{
			SendMessage(player, model.Color, model.Text);
		}
	}

	public void SendInlineMessages(Player player, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator)
	{
		var models = messageCreator(factory);
		var stringBuilder = new StringBuilder(144);
		foreach (var model in models)
		{
			stringBuilder.AppendFormat("{{{0}}}{1} ", model.Color.ToString(), model.Text);
		}
		stringBuilder.Remove(stringBuilder.Length - 1, 1);
		SendMessage(player, Color.White, stringBuilder.ToString());
	}

    public void SendMessage(Predicate<Player> filter, Func<IChatMessageModelFactory, ChatMessageModel> messageCreator)
    {
        foreach(var i in entityManager.GetComponents<Player>())
        {
            if (filter(i))
            {
                SendMessage(i, messageCreator);
            }
        }
    }

    public void SendMessages(Predicate<Player> filter, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator)
    {
        foreach(var i in entityManager.GetComponents<Player>())
        {
            if (filter(i))
            {
                SendMessages(i, messageCreator);
            }
        }
    }

    public void SendInlineMessages(Predicate<Player> filter, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator)
    {
        foreach(var i in entityManager.GetComponents<Player>())
        {
            if (filter(i))
            {
                SendInlineMessages(i, messageCreator);
            }
        }
    }
}

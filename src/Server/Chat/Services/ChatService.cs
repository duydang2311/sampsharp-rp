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

    private static void SendMessage(Player player, Color color, string text)
    {
        player.SendClientMessage(color, text);
    }

    private static void SendMessage(Player player, ChatMessageModel model)
    {
        player.SendClientMessage(model.Color, model.Text);
    }

    private static void SendMessages(Player player, ChatMessageModel[] models)
    {
        foreach (var model in models)
        {
            SendMessage(player, model);
        }
    }

    private static void SendInlineMessages(Player player, ChatMessageModel[] models)
    {
        var stringBuilder = new StringBuilder(144);
        foreach (var model in models)
        {
            stringBuilder.AppendFormat("{{{0}}}{1} ", model.Color.ToString(), model.Text);
        }
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        SendMessage(player, Color.White, stringBuilder.ToString());
    }

	public void SendMessage(Player player, Func<IChatMessageModelFactory, ChatMessageModel> messageCreator)
	{
        SendMessage(player, messageCreator(factory));
	}

	public void SendMessages(Player player, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator)
	{
        SendMessages(player, messageCreator(factory));
	}

	public void SendInlineMessages(Player player, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator)
	{
        SendInlineMessages(player, messageCreator(factory));
	}

    public void SendMessage(Predicate<Player> filter, Func<IChatMessageModelFactory, ChatMessageModel> messageCreator)
    {
        var model = messageCreator(factory);
        foreach(var i in entityManager.GetComponents<Player>())
        {
            if (filter(i))
            {
                SendMessage(i, model);
            }
        }
    }

    public void SendMessages(Predicate<Player> filter, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator)
    {
        var model = messageCreator(factory);
        foreach(var i in entityManager.GetComponents<Player>())
        {
            if (filter(i))
            {
                SendMessages(i, model);
            }
        }
    }

    public void SendInlineMessages(Predicate<Player> filter, Func<IChatMessageModelFactory, ChatMessageModel[]> messageCreator)
    {
        var model = messageCreator(factory);
        foreach(var i in entityManager.GetComponents<Player>())
        {
            if (filter(i))
            {
                SendInlineMessages(i, model);
            }
        }
    }
}

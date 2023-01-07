using System.Text;
using SampSharp.Entities.SAMP;
using Server.Chat.Models;

namespace Server.Chat.Services;

public sealed class ChatService : IChatService
{
	private readonly IChatMessageModelFactory factory;
	public ChatService(IChatMessageModelFactory factory)
	{
		this.factory = factory;
	}

	private void SendMessage(Player player, Color color, string message)
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
			stringBuilder.AppendFormat("{{0}}{1}", model.Color.ToString(), model.Text);
		}
		SendMessage(player, Color.White, stringBuilder.ToString());
	}
}

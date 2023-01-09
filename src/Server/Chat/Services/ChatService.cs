using System.Linq.Expressions;
using System.Text;
using SampSharp.Entities.SAMP;
using Server.Chat.Models;
using Server.I18n.Localization.Models;
using Server.I18n.Localization.Services;

namespace Server.Chat.Services;

public sealed class ChatService : IChatService
{
	private readonly IChatMessageModelFactory factory;
	private readonly ITextNameIdentifierService identifierService;
	private readonly ITextLocalizerService textLocalizerService;

	public ChatService(IChatMessageModelFactory factory, ITextNameIdentifierService identifierService, ITextLocalizerService textLocalizerService)
	{
		this.factory = factory;
		this.identifierService = identifierService;
		this.textLocalizerService = textLocalizerService;
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
			stringBuilder.AppendFormat("{{0}}{1}", model.Color.ToString(), model.Text);
		}
		SendMessage(player, Color.White, stringBuilder.ToString());
	}

	public void SendMessage(Player player, Expression<Func<ITextNameFakeModel, object>> textIdentifier)
	{
		var name = identifierService.Identify(textIdentifier);
		SendMessage(player, Color.White, textLocalizerService.Get(name));
	}

	public void SendMessage(Player player, Expression<Func<ITextNameFakeModel, object>> textIdentifier, params object[] args)
	{
		var name = identifierService.Identify(textIdentifier);
		SendMessage(player, Color.White, textLocalizerService.Get(name, args));
	}

	public void SendMessage(Player player, Color color, Expression<Func<ITextNameFakeModel, object>> textIdentifier)
	{
		var name = identifierService.Identify(textIdentifier);
		SendMessage(player, color, textLocalizerService.Get(name));
	}

	public void SendMessage(Player player, Color color, Expression<Func<ITextNameFakeModel, object>> textIdentifier, params object[] args)
	{
		var name = identifierService.Identify(textIdentifier);
		SendMessage(player, color, textLocalizerService.Get(name, args));
	}
}

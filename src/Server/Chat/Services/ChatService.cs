using System.Linq.Expressions;
using System.Text;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Models;
using Server.I18N.Localization.Components;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.Chat.Services;

public sealed class ChatService : IChatService
{
	private readonly IChatMessageModelFactory factory;
	private readonly IEntityManager entityManager;
	private readonly ITextNameIdentifierService identifierService;
	private readonly ITextLocalizerService localizerService;

	public ChatService(IChatMessageModelFactory factory, IEntityManager entityManager, ITextNameIdentifierService identifierService, ITextLocalizerService localizerService)
	{
		this.factory = factory;
		this.entityManager = entityManager;
		this.identifierService = identifierService;
		this.localizerService = localizerService;
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
		foreach (var i in entityManager.GetComponents<Player>())
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
		foreach (var i in entityManager.GetComponents<Player>())
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
		foreach (var i in entityManager.GetComponents<Player>())
		{
			if (filter(i))
			{
				SendInlineMessages(i, model);
			}
		}
	}

	public void SendMessage(Player player, Expression<Func<ITextNameFakeModel, object>> textIdentifier)
	{
		SendMessage(player, Color.White, textIdentifier);
	}

	public void SendMessage(Player player, Color color, Expression<Func<ITextNameFakeModel, object>> textIdentifier)
	{
		var component = player.GetComponent<CultureComponent>();
		SendMessage(player, color, localizerService.Get(component.Culture, identifierService.Identify(textIdentifier)));
	}

	public void SendMessage(Player player, Expression<Func<ITextNameFakeModel, object>> textIdentifier, params object[] args)
	{
		SendMessage(player, Color.White, textIdentifier, args);
	}

	public void SendMessage(Player player, Color color, Expression<Func<ITextNameFakeModel, object>> textIdentifier, params object[] args)
	{
		var component = player.GetComponent<CultureComponent>();
		SendMessage(player, color, localizerService.Get(component.Culture, identifierService.Identify(textIdentifier), args));
	}
}

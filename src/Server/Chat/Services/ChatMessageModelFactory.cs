using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.Chat.Models;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.Chat.Services;

public sealed class ChatMessageModelFactory : IChatMessageModelFactory
{
	private readonly ITextNameIdentifierService identifierService;
	private readonly ITextLocalizerService localizerService;

	public ChatMessageModelFactory(ITextNameIdentifierService identifierService, ITextLocalizerService localizerService)
	{
		this.identifierService = identifierService;
		this.localizerService = localizerService;
	}

	public ChatMessageModel Create(Expression<Func<ITextNameFakeModel, object>> identifier)
	{
		return Create(Color.White, identifier);
	}

	public ChatMessageModel Create(Color color, Expression<Func<ITextNameFakeModel, object>> identifier)
	{
		return new ChatMessageModel()
		{
			Color = color,
			Text = localizerService.Get(identifierService.Identify(identifier))
		};
	}

	public ChatMessageModel Create(Expression<Func<ITextNameFakeModel, object>> identifier, params object[] args)
	{
		return Create(Color.White, identifier, args);
	}

	public ChatMessageModel Create(Color color, Expression<Func<ITextNameFakeModel, object>> identifier, params object[] args)
	{
		return new ChatMessageModel()
		{
			Color = color,
			Text = localizerService.Get(identifierService.Identify(identifier), args)
		};
	}

	public ChatMessageModel Create(string text)
	{
		return Create(Color.White, text);
	}

	public ChatMessageModel Create(Color color, string text)
	{
		return new ChatMessageModel() { Color = color, Text = text };
	}
}

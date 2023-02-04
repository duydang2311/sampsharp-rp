using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.Chat.Models;
using Server.Common.Colors;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.Chat.Services;

public sealed class ChatMessageBuilder : IChatMessageBuilder
{
	private class BaseBuilderChatMessageModel : ChatMessageModel
	{
		public bool IsInline { get; init; }
	}

	private class I18NBuilderChatMessageModel : BaseBuilderChatMessageModel
	{
		public object[] Args { get; init; } = Array.Empty<object>();
	}

	private readonly ITextLocalizerService localizerService;
	private readonly ITextNameIdentifierService identifierService;

	private readonly LinkedList<BaseBuilderChatMessageModel
	> models = new();

	public ChatMessageBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService)
	{
		this.localizerService = localizerService;
		this.identifierService = identifierService;
	}

	public IChatMessageBuilder Add(Expression<Func<ITextNameFakeModel, object>> textNameIdentifier,
		params object[] args)
	{
		return Add(SemanticColor.Neutral, textNameIdentifier, args);
	}

	public IChatMessageBuilder Add(Color color, Expression<Func<ITextNameFakeModel, object>> textNameIdentifier,
		params object[] args)
	{
		models.AddLast(new I18NBuilderChatMessageModel()
			{ Color = color, Text = identifierService.Identify(textNameIdentifier), Args = args, IsInline = false });
		return this;
	}

	public IChatMessageBuilder Inline(Expression<Func<ITextNameFakeModel, object>> textNameIdentifier,
		params object[] args)
	{
		return Inline(SemanticColor.Neutral, textNameIdentifier);
	}

	public IChatMessageBuilder Inline(Color color, Expression<Func<ITextNameFakeModel, object>> textNameIdentifier,
		params object[] args)
	{
		models.AddLast(new I18NBuilderChatMessageModel()
			{ Color = color, Text = identifierService.Identify(textNameIdentifier), Args = args, IsInline = true });
		return this;
	}

	private string BuildModelInternal(CultureInfo cultureInfo, BaseBuilderChatMessageModel
		model, BaseBuilderChatMessageModel? lastModel = default)
	{
		var appendedColor = string.Empty;
		if (lastModel is null || lastModel.Color != model.Color)
		{
			appendedColor = model.Color.ToString(ColorFormat.RGB);
		}

		return appendedColor +
		       (model is I18NBuilderChatMessageModel i18NModel
			       ? (i18NModel.Args.Length == 0
				       ? localizerService.Get(cultureInfo, i18NModel.Text)
				       : localizerService.Get(cultureInfo, i18NModel.Text, i18NModel.Args))
			       : model.Text);
	}

	public IEnumerable<string> Build(CultureInfo cultureInfo)
	{
		if (models.Count == 0)
		{
			return Array.Empty<string>();
		}

		var text = BuildModelInternal(cultureInfo, models.First!.Value);
		if (models.Count == 1)
		{
			return new[] { text };
		}

		var list = new LinkedList<string>();
		var lastModel = models.First.Value;
		foreach (var m in models.Skip(1))
		{
			if (!m.IsInline)
			{
				list.AddLast(text);
				text = string.Empty;
				lastModel = null;
			}
			else
			{
				text += ' ';
			}

			text += BuildModelInternal(cultureInfo, m, lastModel);
		}

		if (!string.IsNullOrEmpty(text))
		{
			list.AddLast(text);
		}

		return list;
	}

	public IChatMessageBuilder Add(string text, params object[] args)
	{
		return Add(SemanticColor.Neutral, text, args);
	}

	public IChatMessageBuilder Add(Color color, string text, params object[] args)
	{
		models.AddLast(new BaseBuilderChatMessageModel()
		{
			Color = color, Text = args.Length == 0 ? text : string.Format(CultureInfo.InvariantCulture, text, args),
			IsInline = false
		});
		return this;
	}

	public IChatMessageBuilder Inline(string text, params object[] args)
	{
		return Inline(SemanticColor.Neutral, text, args);
	}

	public IChatMessageBuilder Inline(Color color, string text, params object[] args)
	{
		models.AddLast(new BaseBuilderChatMessageModel()
		{
			Color = color, Text = args.Length == 0 ? text : string.Format(CultureInfo.InvariantCulture, text, args),
			IsInline = true
		});
		return this;
	}
}

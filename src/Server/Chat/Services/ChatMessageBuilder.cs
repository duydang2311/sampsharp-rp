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

	public IChatMessageBuilder Add(Expression<Func<ILocalizedText, object>> textIdentifier,
		params object[] args)
	{
		var name = identifierService.Identify(textIdentifier);
		return AddI18N(ResolveBadgeColorInternal(name), name, false, args);
	}

	public IChatMessageBuilder Add(Color color, Expression<Func<ILocalizedText, object>> textIdentifier,
		params object[] args)
	{
		return AddI18N(color, identifierService.Identify(textIdentifier), false, args);
	}

	public IChatMessageBuilder Inline(Expression<Func<ILocalizedText, object>> textIdentifier,
		params object[] args)
	{
		var name = identifierService.Identify(textIdentifier);
		return AddI18N(ResolveBadgeColorInternal(name), name, true, args);
	}

	public IChatMessageBuilder Inline(Color color, Expression<Func<ILocalizedText, object>> textIdentifier,
		params object[] args)
	{
		return AddI18N(color, identifierService.Identify(textIdentifier), true, args);
	}

	private IChatMessageBuilder AddI18N(Color color, string textName, bool isInline, params object[] args)
	{
		models.AddLast(new I18NBuilderChatMessageModel()
		{ Color = color, Text = textName, Args = args, IsInline = isInline });
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
			Color = color,
			Text = args.Length == 0 ? text : string.Format(CultureInfo.InvariantCulture, text, args),
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
			Color = color,
			Text = args.Length == 0 ? text : string.Format(CultureInfo.InvariantCulture, text, args),
			IsInline = true
		});
		return this;
	}

	private static Color ResolveBadgeColorInternal(string name)
	{
		Color color = SemanticColor.Neutral;
		switch (name)
		{
			case nameof(ILocalizedBadge.Badge_System):
				{
					color = SemanticColor.System;
					break;
				}
			case nameof(ILocalizedBadge.Badge_Help):
				{
					color = SemanticColor.Help;
					break;
				}
			case nameof(ILocalizedBadge.Badge_Success):
				{
					color = SemanticColor.Success;
					break;
				}
			case nameof(ILocalizedBadge.Badge_Warning):
				{
					color = SemanticColor.Warning;
					break;
				}
			case nameof(ILocalizedBadge.Badge_Error):
				{
					color = SemanticColor.Error;
					break;
				}
		}
		return color;
	}
}

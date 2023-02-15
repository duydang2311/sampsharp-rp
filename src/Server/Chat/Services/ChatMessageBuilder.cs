using System.Globalization;
using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.Chat.Models;
using Server.Common.Colors;
using Server.I18N.Localization.Models;
using Server.I18N.Localization.Services;

namespace Server.Chat.Services;

public sealed class ChatMessageBuilder : LocalizedTextBuilder<LocalizedChatMessageModel, ILocalizedText, IChatMessageBuilder>, IChatMessageBuilder
{
	private new readonly LinkedList<LocalizedChatMessageModel> models = new();

	public ChatMessageBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService) : base(localizerService, identifierService) { }

	public override IChatMessageBuilder Add(Expression<Func<ILocalizedText, object>> textIdentifier,
		params object[] args)
	{
		var name = identifierService.Identify(textIdentifier);
		return AddInternal(new LocalizedChatMessageModel
		{
			Color = ResolveBadgeColorInternal(name),
			Text = name,
			IsLocal = true,
			IsInline = false,
			Args = args
		});
	}

	public IChatMessageBuilder Add(Color color, Expression<Func<ILocalizedText, object>> textIdentifier,
		params object[] args)
	{
		return AddInternal(new LocalizedChatMessageModel
		{
			Color = color,
			Text = identifierService.Identify(textIdentifier),
			IsLocal = true,
			IsInline = false,
			Args = args
		});
	}

	public IChatMessageBuilder Inline(Expression<Func<ILocalizedText, object>> textIdentifier,
		params object[] args)
	{
		var name = identifierService.Identify(textIdentifier);
		return AddInternal(new LocalizedChatMessageModel
		{
			Color = ResolveBadgeColorInternal(name),
			Text = identifierService.Identify(textIdentifier),
			IsLocal = true,
			IsInline = true,
			Args = args
		});
	}

	public IChatMessageBuilder Inline(Color color, Expression<Func<ILocalizedText, object>> textIdentifier,
		params object[] args)
	{
		return AddInternal(new LocalizedChatMessageModel
		{
			Color = color,
			Text = identifierService.Identify(textIdentifier),
			IsLocal = true,
			IsInline = true,
			Args = args
		});
	}

	private new IChatMessageBuilder AddInternal(LocalizedChatMessageModel model)
	{
		models.AddLast(model);
		return this;
	}

	private string BuildModelInternal(CultureInfo cultureInfo, LocalizedChatMessageModel model, LocalizedChatMessageModel? lastModel = default)
	{
		var appendedColor = string.Empty;
		if (lastModel is null || lastModel.Color != model.Color)
		{
			appendedColor = model.Color.ToString(ColorFormat.RGB);
		}

		return appendedColor + base.BuildModelInternal(cultureInfo, model);
	}

	public override IEnumerable<string> Build(CultureInfo cultureInfo)
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

	public override IChatMessageBuilder Add(string text, params object[] args)
	{
		return Add(SemanticColor.Neutral, text, args);
	}

	public IChatMessageBuilder Add(Color color, string text, params object[] args)
	{
		return AddInternal(new LocalizedChatMessageModel
		{
			Color = color,
			Text = string.Format(text, args),
			IsLocal = false,
			IsInline = false,
		});
	}

	public IChatMessageBuilder Inline(string text, params object[] args)
	{
		return Inline(SemanticColor.Neutral, text, args);
	}

	public IChatMessageBuilder Inline(Color color, string text, params object[] args)
	{
		return AddInternal(new LocalizedChatMessageModel
		{
			Color = color,
			Text = string.Format(text, args),
			IsLocal = false,
			IsInline = true,
		});
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

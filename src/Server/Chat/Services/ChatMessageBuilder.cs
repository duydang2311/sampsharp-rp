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
		public bool IsInline { get; set; }
	}
	private class I18NBuilderChatMessageModel : BaseBuilderChatMessageModel
	{
		public object[] Args { get; set; } = Array.Empty<object>();
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

	public IChatMessageBuilder Add(Expression<Func<ITextNameFakeModel, object>> textNameIdentifier, params object[] args)
	{
		return Add(SemanticColor.Neutral, textNameIdentifier);
	}

	public IChatMessageBuilder Add(Color color, Expression<Func<ITextNameFakeModel, object>> textNameIdentifier, params object[] args)
	{
		models.AddLast(new I18NBuilderChatMessageModel() { Color = color, Text = identifierService.Identify(textNameIdentifier), Args = args, IsInline = false });
		return this;
	}

	public IChatMessageBuilder Inline(Expression<Func<ITextNameFakeModel, object>> textNameIdentifier, params object[] args)
	{
		return Inline(SemanticColor.Neutral, textNameIdentifier);
	}

	public IChatMessageBuilder Inline(Color color, Expression<Func<ITextNameFakeModel, object>> textNameIdentifier, params object[] args)
	{
		models.AddLast(new I18NBuilderChatMessageModel() { Color = color, Text = identifierService.Identify(textNameIdentifier), Args = args, IsInline = true });
		return this;
	}

	private string BuildModelInternal(CultureInfo cultureInfo, BaseBuilderChatMessageModel
		model)
	{
		return model is I18NBuilderChatMessageModel i18nModel
			? i18nModel.Color.ToString(ColorFormat.RGB) + (i18nModel.Args.Length == 0 ? localizerService.Get(cultureInfo, i18nModel.Text) : localizerService.Get(cultureInfo, i18nModel.Text, i18nModel.Args))
			: model.Color.ToString(ColorFormat.RGB) + model.Text;
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
			return new string[] { text };
		}

		var list = new LinkedList<string>();
		foreach (var m in models.Skip(1))
		{
			if (!m.IsInline)
			{
				list.AddLast(text);
				text = string.Empty;
			}
			else
			{
				text += ' ';
			}
			text += BuildModelInternal(cultureInfo, m);
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
		models.AddLast(new BaseBuilderChatMessageModel() { Color = color, Text = args.Length == 0 ? text : string.Format(CultureInfo.InvariantCulture, text, args), IsInline = false });
		return this;
	}

	public IChatMessageBuilder Inline(string text, params object[] args)
	{
		return Inline(SemanticColor.Neutral, text, args);
	}

	public IChatMessageBuilder Inline(Color color, string text, params object[] args)
	{
		models.AddLast(new BaseBuilderChatMessageModel() { Color = color, Text = args.Length == 0 ? text : string.Format(CultureInfo.InvariantCulture, text, args), IsInline = true });
		return this;
	}
}

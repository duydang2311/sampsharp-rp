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
	private class BuilderChatMessageModel : ChatMessageModel
	{
		public bool IsInline { get; set; }
		public object[] Args { get; set; } = Array.Empty<object>();
	}
	private readonly ITextLocalizerService localizerService;
	private readonly ITextNameIdentifierService identifierService;
	private readonly LinkedList<BuilderChatMessageModel> models = new();

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
		models.AddLast(new BuilderChatMessageModel() { Color = color, Text = identifierService.Identify(textNameIdentifier), Args = args, IsInline = false });
		return this;
	}

	public IChatMessageBuilder Inline(Expression<Func<ITextNameFakeModel, object>> textNameIdentifier, params object[] args)
	{
		return Inline(SemanticColor.Neutral, textNameIdentifier);
	}

	public IChatMessageBuilder Inline(Color color, Expression<Func<ITextNameFakeModel, object>> textNameIdentifier, params object[] args)
	{
		models.AddLast(new BuilderChatMessageModel() { Color = color, Text = identifierService.Identify(textNameIdentifier), Args = args, IsInline = true });
		return this;
	}

	private string BuildModelInternal(CultureInfo cultureInfo, BuilderChatMessageModel model)
	{
		return model.Color.ToString(ColorFormat.RGB) + localizerService.Get(cultureInfo, model.Text, model.Args);
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
		return list;
	}
}

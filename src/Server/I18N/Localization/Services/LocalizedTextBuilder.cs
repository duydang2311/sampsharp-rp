using System.Globalization;
using System.Linq.Expressions;
using Server.I18N.Localization.Models;

namespace Server.I18N.Localization.Services;

public class LocalizedTextBuilder : ILocalizedTextBuilder
{
	private readonly ITextLocalizerService localizerService;
	private readonly ITextNameIdentifierService identifierService;

	private readonly LinkedList<object> models = new();

	public LocalizedTextBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService)
	{
		this.localizerService = localizerService;
		this.identifierService = identifierService;
	}

	public ILocalizedTextBuilder Add(Expression<Func<ILocalizedText, object>> textIdentifier,
		params object[] args)
	{
		return AddI18N(identifierService.Identify(textIdentifier), args);
	}

	public ILocalizedTextBuilder Add(string text)
	{
		models.AddLast(text);
		return this;
	}

	public ILocalizedTextBuilder Add(string text, params object[] args)
	{
		return Add(string.Format(text, args));
	}

	private ILocalizedTextBuilder AddI18N(string textName, params object[] args)
	{
		models.AddLast(new LocalizedTextModel()
		{ Text = textName, Args = args });
		return this;
	}

	private string BuildModelInternal(CultureInfo cultureInfo, object
		model, object? lastModel = default)
	{
		return model is LocalizedTextModel localizedModel
			? (localizedModel.Args.Length == 0
				? localizerService.Get(cultureInfo, localizedModel.Text)
				: localizerService.Get(cultureInfo, localizedModel.Text, localizedModel.Args))
			: (string)model;
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
			list.AddLast(text);
			text = string.Empty;
			lastModel = null;
			text += BuildModelInternal(cultureInfo, m, lastModel);
		}

		if (!string.IsNullOrEmpty(text))
		{
			list.AddLast(text);
		}

		return list;
	}
}

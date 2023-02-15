using System.Globalization;
using System.Linq.Expressions;
using Server.I18N.Localization.Models;

namespace Server.I18N.Localization.Services;

public abstract class LocalizedTextBuilder<TModel, TInterface, TBuilder> : ILocalizedTextBuilder<TModel, TInterface, TBuilder>
	where TModel : LocalizedTextModel, new()
	where TInterface : class
	where TBuilder : ILocalizedTextBuilder<TModel, TInterface, TBuilder>
{
	protected readonly ITextLocalizerService localizerService;
	protected readonly ITextNameIdentifierService identifierService;
	protected readonly LinkedList<TModel> models = new();
	protected readonly TBuilder _this;

	public LocalizedTextBuilder(ITextLocalizerService localizerService, ITextNameIdentifierService identifierService)
	{
		this.localizerService = localizerService;
		this.identifierService = identifierService;
		_this = (TBuilder)(ILocalizedTextBuilder<TModel, TInterface, TBuilder>)this;
	}

	public virtual TBuilder Add(Expression<Func<TInterface, object>> textIdentifier,
		params object[] args)
	{
		return AddInternal(new TModel { Text = identifierService.Identify(textIdentifier), IsLocal = true, Args = args });
	}

	public virtual TBuilder Add(string text)
	{
		return AddInternal(new TModel { Text = text, IsLocal = false });
	}

	public virtual TBuilder Add(string text, params object[] args)
	{
		return AddInternal(new TModel { Text = string.Format(text, args) });
	}

	protected virtual TBuilder AddInternal(TModel model)
	{
		models.AddLast(model);
		return _this;
	}

	protected virtual string BuildModelInternal(CultureInfo cultureInfo, TModel model)
	{
		return model.IsLocal
			? (model.Args.Length == 0
				? localizerService.Get(cultureInfo, model.Text)
				: localizerService.Get(cultureInfo, model.Text, model.Args))
			: model.Text;
	}

	public virtual IEnumerable<string> Build(CultureInfo cultureInfo)
	{
		if (models.Count == 0)
		{
			return Array.Empty<string>();
		}

		var list = new LinkedList<string>();
		foreach (var m in models)
		{
			list.AddLast(BuildModelInternal(cultureInfo, m));
		}

		return list;
	}
}

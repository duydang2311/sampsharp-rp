using System.Linq.Expressions;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Components;
using Server.I18N.Localization.Models;

namespace Server.I18N.Localization.Services;

public sealed class PlayerTextLocalizerService : IPlayerTextLocalizerService
{
	private readonly ITextNameIdentifierService textNameIdentifierService;
	private readonly ITextLocalizerServiceOptions textLocalizerServiceOptions;
	private readonly ITextLocalizerService textLocalizerService;

	public PlayerTextLocalizerService(ITextNameIdentifierService textNameIdentifierService, ITextLocalizerServiceOptions textLocalizerServiceOptions, ITextLocalizerService textLocalizerService)
	{
		this.textNameIdentifierService = textNameIdentifierService;
		this.textLocalizerServiceOptions = textLocalizerServiceOptions;
		this.textLocalizerService = textLocalizerService;
	}

	public string Get(Player player, Expression<Func<ILocalizedText, object>> textIdentifier, params object[] args)
	{
		var component = player.GetComponent<CultureComponent>();
		var culture = component?.Culture ?? textLocalizerServiceOptions.DefaultCulture;
		return textLocalizerService.Get(culture, textNameIdentifierService.Identify(textIdentifier), args);
	}
}

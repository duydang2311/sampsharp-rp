using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.I18n.Localization.Components;
using Server.I18N.Localization.Services;

namespace Server.I18N.Localization.Systems.Connect;

public sealed class ConnectSystem : ISystem
{
	[Event]
	private void OnPlayerConnect(Player player, ITextLocalizerServiceOptions localizerOptions)
	{
		player.AddComponent<CultureComponent>(localizerOptions.DefaultCulture);
	}
}

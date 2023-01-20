using System.Globalization;
using SampSharp.Entities;

namespace Server.I18n.Localization.Components;

public class CultureComponent : Component
{
	public CultureInfo Culture { get; set; }

	public CultureComponent(CultureInfo culture)
	{
		Culture = culture;
	}
}

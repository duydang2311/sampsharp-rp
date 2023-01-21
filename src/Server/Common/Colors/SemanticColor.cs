using SampSharp.Entities.SAMP;

namespace Server.Common.Colors;

public static class SemanticColor
{
	public static Color Roleplay => new(0xC2, 0xA2, 0xDA);
	public static Color Info => new(0, 0, 0xFF);
	public static Color Success => new(0, 0xFF, 0);
	public static Color Error => new(0xFF, 0, 0);
	public static Color Neutral => Color.White;
	public static Color LowAttention => Color.SlateGray;
}

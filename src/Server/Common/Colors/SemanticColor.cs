using SampSharp.Entities.SAMP;

namespace Server.Common.Colors;

public static class SemanticColor
{
	public static Color Roleplay => new(0xC2, 0xA2, 0xDA);
	public static Color Info => new(0, 0, 0xFF);
	public static Color Success => new(34, 197, 94);
	public static Color Error => new(239, 68, 68);
	public static Color Warning => new(234, 179, 8);
	public static Color Neutral => new(203, 213, 225);
	public static Color LowAttention => new(107, 114, 128);
	public static Color Help => new(0x27, 0xBE, 0xF0);
	public static Color System => new(163, 163, 163);
	public static Color ChatMessage => new(241, 245, 249);
}

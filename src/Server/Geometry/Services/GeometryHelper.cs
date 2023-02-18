using SampSharp.Entities.SAMP;

namespace Server.Geometry.Services;

public static class GeometryHelper
{
	public static void GetLineFormula(Vector2 point1, Vector2 point2, out float a, out float b, out float c)
	{
		a = point1.Y - point2.Y;
		b = point2.X - point1.X;
		c = point1.X * point2.Y - point1.Y * point2.X;
	}

	public static float GetDistanceSquaredToLine(float a, float b, float c, Vector2 point)
	{
		var formula = a * point.X + b * point.Y + c;
		return formula * formula / (a * a + b * b);
	}
}

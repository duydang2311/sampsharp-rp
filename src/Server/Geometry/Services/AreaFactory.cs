using SampSharp.Entities.SAMP;
using Server.Geometry.Entities;

namespace Server.Geometry.Services;

public sealed class AreaFactory : IAreaFactory
{
	public ICircleArea CreateCircle(float centerX, float centerY, float radius)
	{
		return new CircleArea(centerX - radius, centerY + radius) { Radius = radius };
	}

	public IRectangleArea CreateRectangle(float left, float top, float right, float bottom)
	{
		return CreateRectangle(new Vector2(left, top), new Vector2(right, bottom));
	}

	public IRectangleArea CreateRectangle(Vector2 leftTop, Vector2 rightBottom)
	{
		return new RectangleArea(leftTop, rightBottom);
	}

	public IPolygonArea CreatePolygon(params Vector2[] points)
	{
		var area = new PolygonArea();
		area.AddRange(points);
		return area;
	}
}

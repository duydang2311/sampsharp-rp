using SampSharp.Entities.SAMP;
using Server.Geometry.Entities;

namespace Server.Geometry.Services;

public interface IAreaFactory
{
	ICircleArea CreateCircle(float centerX, float centerY, float radius);
	IRectangleArea CreateRectangle(float left, float top, float right, float bottom);
	IRectangleArea CreateRectangle(Vector2 topLeft, Vector2 rightBottom);
	IPolygonArea CreatePolygon(params Vector2[] points);
}

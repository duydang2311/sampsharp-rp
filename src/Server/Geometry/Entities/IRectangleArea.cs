using SampSharp.Entities.SAMP;

namespace Server.Geometry.Entities;

public interface IRectangleArea : IPolygonArea
{
	Vector2 BottomRight { get; }
}

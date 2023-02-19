using SampSharp.Entities.SAMP;

namespace Server.Geometry.Entities;

public interface IArea
{
	Vector2 LeftTop { get; }
	Vector2 Center { get; }
	Vector2 RightBottom { get; }
	bool Contains(Vector2 position);
	bool Overlaps(IArea other);
	bool Overlaps(ICircleArea other);
	bool Overlaps(IPolygonArea other);
	bool Overlaps(IRectangleArea other);
}

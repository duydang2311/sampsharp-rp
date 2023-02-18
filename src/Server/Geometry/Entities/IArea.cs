using SampSharp.Entities.SAMP;

namespace Server.Geometry.Entities;

public interface IArea
{
	bool Contains(Vector2 position);
	bool Overlaps(IArea other);
	bool Overlaps(ICircleArea other);
	bool Overlaps(IPolygonArea other);
	bool Overlaps(IRectangleArea other);
}

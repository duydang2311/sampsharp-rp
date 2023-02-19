using SampSharp.Entities.SAMP;

namespace Server.Geometry.Entities;

public sealed class CircleArea : ICircleArea
{
	public Vector2 TopLeft { get; }
	public Vector2 Center => TopLeft + new Vector2(Radius);
	public float Radius { get; set; }
	public float RadiusSquared => Radius * Radius;

	public bool Contains(Vector2 position)
	{
		return Vector2.DistanceSquared(Center, position) <= RadiusSquared;
	}

	public bool Overlaps(IArea other)
	{
		return other.Overlaps(this);
	}

	public bool Overlaps(ICircleArea other)
	{
		var totalRadius = Radius + other.Radius;
		return Vector2.DistanceSquared(Center, other.Center) <= 4 * totalRadius * totalRadius;
	}

	public bool Overlaps(IPolygonArea other)
	{
		return other.Overlaps(this);
	}

	public bool Overlaps(IRectangleArea other)
	{
		return other.Overlaps(this);
	}
}

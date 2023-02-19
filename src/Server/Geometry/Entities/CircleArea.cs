using SampSharp.Entities.SAMP;

namespace Server.Geometry.Entities;

public sealed class CircleArea : ICircleArea
{
	private readonly float radius;
	private readonly Vector2 center;
	private readonly Vector2 bottomRight;

	public Vector2 TopLeft { get; }
	public Vector2 BottomRight => bottomRight;
	public Vector2 Center => center;
	public float Radius => radius;
	public float RadiusSquared => radius * radius;

	public CircleArea(float left, float top)
	{
		TopLeft = new(left, top);
		center = TopLeft + new Vector2(Radius, -Radius);
		bottomRight = TopLeft + new Vector2(2 * Radius, -2 * Radius);
	}

	public CircleArea(double left, double top) : this((float)left, (float)top) { }

	public bool Contains(Vector2 position)
	{
		return Vector2.DistanceSquared(center, position) <= RadiusSquared;
	}

	public bool Overlaps(IArea other)
	{
		return other.Overlaps(this);
	}

	public bool Overlaps(ICircleArea other)
	{
		var totalRadius = Radius + other.Radius;
		return Vector2.DistanceSquared(center, other.Center) <= 4 * totalRadius * totalRadius;
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

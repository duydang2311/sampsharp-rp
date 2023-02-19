using SampSharp.Entities.SAMP;

namespace Server.Geometry.Entities;

public sealed class CircleArea : ICircleArea
{
	private float radius;
	private Vector2 center;
	private Vector2 bottomRight;

	public Vector2 LeftTop { get; }
	public Vector2 Center => center;
	public Vector2 RightBottom => bottomRight;
	public float Radius
	{
		get => radius; set
		{
			radius = value;
			center = LeftTop + new Vector2(radius, -radius);
			bottomRight = LeftTop + new Vector2(2 * radius, -2 * radius);
		}
	}
	public float RadiusSquared => Radius * Radius;

	public CircleArea(float left, float top)
	{
		LeftTop = new(left, top);
		center = LeftTop + new Vector2(radius, -radius);
		bottomRight = LeftTop + new Vector2(2 * radius, -2 * radius);
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

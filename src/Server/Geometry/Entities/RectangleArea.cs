using SampSharp.Entities.SAMP;

namespace Server.Geometry.Entities;

public sealed class RectangleArea : PolygonArea, IRectangleArea
{
	public Vector2 TopLeft { get; set; }
	public Vector2 BottomRight { get; set; }

	public override void Add(Vector2 point)
	{
		if (points.Count >= 4)
		{
			return;
		}
		base.Add(point);
	}

	public override void AddRange(params Vector2[] points)
	{
		var currentCount = this.points.Count;
		if (currentCount >= 4)
		{
			return;
		}
		base.AddRange(points.Take(4 - currentCount).ToArray());
	}

	public override bool Contains(Vector2 position)
	{
		return position.X >= TopLeft.X
			&& position.X <= BottomRight.X
			&& position.Y >= TopLeft.Y
			&& position.Y <= BottomRight.Y;
	}

	public override bool Overlaps(IArea other)
	{
		return other.Overlaps(this);
	}

	public override bool Overlaps(ICircleArea other)
	{
		var closestX = Math.Clamp(other.Center.X, TopLeft.X, BottomRight.X);
		var closestY = Math.Clamp(other.Center.Y, TopLeft.Y, BottomRight.Y);
		var dx = other.Center.X - closestX;
		var dy = other.Center.Y - closestY;
		return dx * dx + dy * dy < other.RadiusSquared;
	}

	public override bool Overlaps(IPolygonArea other)
	{
		return base.Overlaps(other);
	}

	public override bool Overlaps(IRectangleArea other)
	{
		return TopLeft.X < other.BottomRight.X
			&& BottomRight.X > other.TopLeft.X
			&& TopLeft.Y > other.BottomRight.Y
			&& BottomRight.Y < other.TopLeft.Y;
	}
}

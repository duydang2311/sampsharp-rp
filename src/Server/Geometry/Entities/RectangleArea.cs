using SampSharp.Entities.SAMP;

namespace Server.Geometry.Entities;

public sealed class RectangleArea : PolygonArea, IRectangleArea
{
	public override Vector2 LeftTop { get; }
	public override Vector2 Center { get; }
	public override Vector2 RightBottom { get; }

	public RectangleArea(Vector2 leftTop, Vector2 rightBottom)
	{
		LeftTop = leftTop;
		Center = new Vector2(leftTop.X + (rightBottom.X - leftTop.X) / 2, leftTop.Y - (leftTop.Y - rightBottom.Y) / 2);
		RightBottom = rightBottom;
		RestorePoints();
	}

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
		return position.X >= LeftTop.X
			&& position.X <= RightBottom.X
			&& position.Y <= LeftTop.Y
			&& position.Y >= RightBottom.Y;
	}

	public override bool Overlaps(IArea other)
	{
		return other.Overlaps(this);
	}

	public override bool Overlaps(ICircleArea other)
	{
		var closestX = Math.Clamp(other.Center.X, LeftTop.X, RightBottom.X);
		var closestY = Math.Clamp(other.Center.Y, RightBottom.Y, LeftTop.Y);
		var dx = other.Center.X - closestX;
		var dy = other.Center.Y - closestY;
		return dx * dx + dy * dy <= other.RadiusSquared;
	}

	public override bool Overlaps(IPolygonArea other)
	{
		return base.Overlaps(other);
	}

	public override bool Overlaps(IRectangleArea other)
	{
		return LeftTop.X < other.RightBottom.X
			&& RightBottom.X > other.LeftTop.X
			&& LeftTop.Y > other.RightBottom.Y
			&& RightBottom.Y < other.LeftTop.Y;
	}

	private void RestorePoints()
	{
		points.Clear();
		AddRange(new Vector2[]
		{
			new Vector2(LeftTop.X, LeftTop.Y),
			new Vector2(RightBottom.X, LeftTop.Y),
			new Vector2(RightBottom.X, RightBottom.Y),
			new Vector2(LeftTop.X, RightBottom.Y),
		});
	}
}

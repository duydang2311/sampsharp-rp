using SampSharp.Entities.SAMP;
using Server.Geometry.Services;

namespace Server.Geometry.Entities;

public class PolygonArea : IPolygonArea
{
	protected List<Vector2> points = new();

	public virtual Vector2 TopLeft => points.Count > 0 ? points[0] : default;
	public IReadOnlyCollection<Vector2> Points => points;

	public virtual bool Contains(Vector2 position)
	{
		if (points.Count < 3)
		{
			return false;
		}

		var contains = false;
		for (int i = 0, j = points.Count - 1, count = points.Count; i != count; ++i)
		{
			var last = points[j];
			var cur = points[i];
			if ((cur.Y > position.Y ^ last.Y > position.Y) && (position.X < (last.X - cur.X) * (position.Y - cur.Y) / (last.Y - cur.Y) + cur.X))
			{
				contains = !contains;
			}
			j = i;
		}
		return contains;
	}

	public virtual void Add(Vector2 point)
	{
		points.Add(point);
	}

	public virtual void AddRange(params Vector2[] points)
	{
		this.points.AddRange(points);
	}

	public virtual void Remove(Vector2 point)
	{
		points.Remove(point);
	}

	public virtual int Remove(Predicate<Vector2> filter)
	{
		var removedCount = points.RemoveAll(filter);
		return removedCount;
	}

	public virtual void RemoveAt(int index)
	{
		points.RemoveAt(index);
	}

	public virtual bool Overlaps(IArea other)
	{
		if (points.Count < 3)
		{
			return false;
		}

		return other.Overlaps(this);
	}

	public virtual bool Overlaps(ICircleArea other)
	{
		if (points.Count < 3)
		{
			return false;
		}

		if (Contains(other.Center))
		{
			return true;
		}

		foreach (var point in points)
		{
			if (other.Contains(point))
			{
				return true;
			}
		}

		var center = other.Center;
		for (int i = 0, count = points.Count, j = count - 1; i != count; ++i)
		{
			var last = points[j];
			var cur = points[i];
			GeometryHelper.GetLineFormula(cur, last, out var a, out var b, out var c);
			if (GeometryHelper.GetDistanceSquaredToLine(a, b, c, center) <= other.RadiusSquared)
			{
				var minX = Math.Min(cur.X, last.X);
				var maxX = Math.Max(cur.X, last.X);
				var minY = Math.Min(cur.Y, last.Y);
				var maxY = Math.Max(cur.Y, last.Y);
				if (center.X >= minX && center.X <= maxX && center.Y >= minY && center.Y <= maxY)
				{
					return true;
				}
			}
			j = i;
		}
		return false;
	}

	public virtual bool Overlaps(IPolygonArea other)
	{
		if (points.Count < 3)
		{
			return false;
		}

		var otherPoints = other.Points.ToList();
		for (int i1 = 0, count1 = otherPoints.Count, j1 = count1 - 1; i1 != count1; ++i1)
		{
			for (int i2 = 0, count2 = points.Count, j2 = count2 - 1; i2 != count2; ++i2)
			{
				if (EdgeIntersects(otherPoints[i1], otherPoints[j1], points[i2], points[j2]))
				{
					return true;
				}
				j2 = i2;
			}
			j1 = i1;
		}

		foreach (var point in points)
		{
			if (other.Contains(point))
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool Overlaps(IRectangleArea other)
	{
		if (points.Count < 3)
		{
			return false;
		}

		return other.Overlaps(this);
	}

	private static bool EdgeIntersects(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
	{
		var denom = (d.Y - c.Y) * (b.X - a.X) - (d.X - c.X) * (b.Y - a.Y);
		var numeA = (d.X - c.X) * (a.Y - c.Y) - (d.Y - c.Y) * (a.X - c.X);
		var numeB = (b.X - a.X) * (a.Y - c.Y) - (b.Y - a.Y) * (a.X - c.X);
		if (denom == 0 || (numeA == 0 && numeB == 0))
		{
			return false;
		}

		var uA = numeA / denom;
		var uB = numeB / denom;
		return uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1;
	}
}

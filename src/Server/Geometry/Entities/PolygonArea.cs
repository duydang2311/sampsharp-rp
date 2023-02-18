using SampSharp.Entities.SAMP;
using Server.Geometry.Services;

namespace Server.Geometry.Entities;

public class PolygonArea : IPolygonArea
{
	protected List<Vector2> points = new();
	protected HashSet<Vector2> rightNormals = new();

	public IReadOnlyCollection<Vector2> Points => points;
	public IReadOnlyCollection<Vector2> RightNormals => rightNormals;

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
		RestoreRightNormals();
	}

	public virtual void AddRange(params Vector2[] points)
	{
		this.points.AddRange(points);
		RestoreRightNormals();
	}

	public virtual void Remove(Vector2 point)
	{
		points.Remove(point);
		RestoreRightNormals();
	}

	public virtual int Remove(Predicate<Vector2> filter)
	{
		var removedCount = points.RemoveAll(filter);
		RestoreRightNormals();
		return removedCount;
	}

	public virtual void RemoveAt(int index)
	{
		points.RemoveAt(index);
		RestoreRightNormals();
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

		float a, b, c;
		for (int i = 0, j = points.Count - 1, count = points.Count; i != count; ++i)
		{
			var last = points[j];
			var cur = points[i];
			GeometryHelper.GetLineFormula(cur, last, out a, out b, out c);
			if (GeometryHelper.GetDistanceSquaredToLine(a, b, c, other.Center) <= other.RadiusSquared)
			{
				return true;
			}
			last = cur;
		}
		return false;
	}

	public virtual bool Overlaps(IPolygonArea other)
	{
		if (points.Count < 3)
		{
			return false;
		}

		foreach (var normal in RightNormals)
		{
			var mm1 = FindMaxMinProjection(this, normal);
			var mm2 = FindMaxMinProjection(other, normal);
			if (mm1.Max < mm2.Min || mm2.Max < mm1.Min)
			{
				return false;
			}
		}
		foreach (var normal in other.RightNormals)
		{
			var mm1 = FindMaxMinProjection(this, normal);
			var mm2 = FindMaxMinProjection(other, normal);
			if (mm1.Max < mm2.Min || mm2.Max < mm1.Min)
			{
				return false;
			}
		}
		return true;
	}

	public virtual bool Overlaps(IRectangleArea other)
	{
		if (points.Count < 3)
		{
			return false;
		}

		return other.Overlaps(this);
	}

	private static Vector2[] GetRightNormals(IPolygonArea area)
	{
		var points = area.Points.ToArray();
		var rightNormals = new Vector2[points.Length];
		for (int i = 0, count = points.Length, j = count - 1; i != count; ++i)
		{
			var edge = points[i] - points[j];
			rightNormals[i] = new Vector2(edge.Y, -edge.X).Normalized();
		}
		return rightNormals;
	}

	private static MinMax FindMaxMinProjection(IPolygonArea area, Vector2 axis)
	{
		var points = area.Points.ToArray();
		var projection = Vector2.Dot(points[0], axis);
		var max = projection;
		var min = projection;
		for (int i = 1, length = points.Length; i != length; ++i)
		{
			projection = Vector2.Dot(points[i], axis);
			max = max > projection ? max : projection;
			min = min < projection ? min : projection;
		}
		return new MinMax(min, max);
	}

	private void RestoreRightNormals()
	{
		rightNormals.Clear();
		for (int i = 0, count = points.Count, j = count - 1; i != count; ++i)
		{
			var edge = points[i] - points[j];
			rightNormals.Add(new Vector2(edge.Y, -edge.X).Normalized());
			j = i;
		}
	}

	private struct MinMax
	{
		public float Min;
		public float Max;
		public MinMax(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}
}

using SampSharp.Entities.SAMP;

namespace Server.Geometry.Entities;

public interface IPolygonArea : IArea
{
	IReadOnlyCollection<Vector2> Points { get; }
	IReadOnlyCollection<Vector2> RightNormals { get; }
	void Add(Vector2 point);
	void AddRange(params Vector2[] points);
	void Remove(Vector2 point);
	int Remove(Predicate<Vector2> filter);
	void RemoveAt(int index);
}

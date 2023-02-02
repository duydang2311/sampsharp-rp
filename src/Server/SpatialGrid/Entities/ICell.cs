using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public interface ICell : IBaseCell
{
	IEnumerable<BaseSpatialComponent> Components { get; }
	void Add(BaseSpatialComponent component);
	bool Remove(BaseSpatialComponent component);
	void Clear();
}

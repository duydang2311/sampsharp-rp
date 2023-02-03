using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public interface IBaseCell
{
	float Top { get; }
	float Left { get; }
	bool Add(BaseSpatialComponent component);
	bool Remove(BaseSpatialComponent component);
	void Clear();
}

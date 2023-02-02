using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public interface IBaseCell
{
	bool Add(BaseSpatialComponent component);
	bool Remove(BaseSpatialComponent component);
	void Clear();
}

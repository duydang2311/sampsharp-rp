using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public interface IBaseCell
{
	int Add(ISpatialComponent component);
	int Remove(ISpatialComponent component);
	void Clear();
}

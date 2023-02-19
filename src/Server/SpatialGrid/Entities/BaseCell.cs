using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public abstract class BaseCell : IBaseCell
{
	public BaseCell() { }

	public abstract int Add(ISpatialComponent component);
	public abstract int Remove(ISpatialComponent component);
	public abstract void Clear();
}

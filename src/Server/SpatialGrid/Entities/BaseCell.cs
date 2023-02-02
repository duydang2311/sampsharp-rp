using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public abstract class BaseCell : IBaseCell
{
	public abstract bool Add(BaseSpatialComponent component);
	public abstract void Clear();
	public abstract bool Remove(BaseSpatialComponent component);
}

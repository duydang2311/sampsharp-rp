using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public abstract class BaseCell : IBaseCell
{
	public float Top { get; }
	public float Left { get; }

	public BaseCell(float top, float left)
	{
		Top = top;
		Left = left;
	}

	public abstract bool Add(BaseSpatialComponent component);
	public abstract bool Remove(BaseSpatialComponent component);
	public abstract void Clear();
}

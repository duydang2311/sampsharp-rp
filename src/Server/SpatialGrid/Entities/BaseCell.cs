using SampSharp.Entities.SAMP;
using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public abstract class BaseCell : IBaseCell
{
	public Vector2 Start { get; }

	public BaseCell(float x, float y)
	{
		Start = new Vector2(x, y);
	}

	public abstract bool Add(BaseSpatialComponent component);
	public abstract bool Remove(BaseSpatialComponent component);
	public abstract void Clear();
}

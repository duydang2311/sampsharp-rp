using SampSharp.Entities.SAMP;
using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public interface IBaseCell
{
	Vector2 Start { get; }
	bool Add(BaseSpatialComponent component);
	bool Remove(BaseSpatialComponent component);
	void Clear();
}

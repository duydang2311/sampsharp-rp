using Server.Geometry.Entities;

namespace Server.SpatialGrid.Components;

public abstract class BaseSpatialComponent : ISpatialComponent
{
	public IArea Area { get; set; }

	public BaseSpatialComponent(IArea area)
	{
		Area = area;
	}
}

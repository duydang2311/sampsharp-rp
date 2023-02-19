using Server.Geometry.Entities;

namespace Server.SpatialGrid.Components;

public interface ISpatialComponent
{
	IArea Area { get; set; }
}

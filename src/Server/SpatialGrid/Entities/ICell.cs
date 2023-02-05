using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public interface ICell : IBaseCell
{
	IEnumerable<ISpatialComponent> Components { get; }
}

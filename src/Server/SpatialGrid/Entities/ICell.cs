using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public interface ICell : IBaseCell
{
	IReadOnlyCollection<ISpatialComponent> Components { get; }
}

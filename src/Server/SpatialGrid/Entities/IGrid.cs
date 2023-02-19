using SampSharp.Entities.SAMP;
using Server.Geometry.Entities;
using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public interface IGrid : IBaseCell
{
	IRectangleArea Area { get; }
	int Columns { get; }
	int Rows { get; }
	IEnumerable<ISpatialComponent> FindComponents(Vector2 position, float radius);
	IEnumerable<IBaseCell> FindCells(Vector2 position, float radius);
}

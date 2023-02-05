using SampSharp.Entities.SAMP;
using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public interface IGrid : IBaseCell
{
	Vector2 End { get; }
	float CellWidth { get; }
	float CellHeight { get; }
	int Columns { get; }
	int Rows { get; }
	IEnumerable<ISpatialComponent> FindComponents(Vector2 position, float radius);
	IEnumerable<IBaseCell> FindCells(Vector2 position, float radius);
	bool TryComputeIndex(Vector2 position, out int row, out int column);
}

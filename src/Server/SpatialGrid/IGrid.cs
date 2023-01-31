using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace Server.SpatialGrid;

public interface IGrid : IBaseCell
{
	float Top { get; }
	float Left { get; }
	float Right { get; }
	float Bottom { get; }
	float CellWidth { get; }
	float CellHeight { get; }
	int Columns { get; }
	int Rows { get; }
	IEnumerable<IBaseCell> GetSurroundingCells(int row, int column);
	IBaseCell? FindCell(Vector2 position);
	IBaseCell? FindCell(Predicate<IBaseCell> cellPredicate);
	bool TryComputeIndex(Vector2 position, out int row, out int column);
	bool Add(Vector2 position, Component component);
	bool Remove(Vector2 position, Component component);
}

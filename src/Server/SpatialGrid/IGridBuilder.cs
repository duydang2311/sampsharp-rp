namespace Server.SpatialGrid;

public interface IGridBuilder
{
	float Top { get; }
	float Left { get; }
	float Right { get; }
	float Bottom { get; }
	int Rows { get; }
	int Columns { get; }
	IDictionary<(int Row, int Column), IGridBuilder> InnerGrids { get; }
	IGridBuilder SetTop(float top);
	IGridBuilder SetLeft(float left);
	IGridBuilder SetRight(float right);
	IGridBuilder SetBottom(float bottom);
	IGridBuilder SetRows(int rows);
	IGridBuilder SetColumns(int columns);
	IGridBuilder SetAsInnerGrid(int row, int column, Func<IGridBuilder, IGridBuilder> gridBuilderOptions);
	IGrid BuildGrid();
}

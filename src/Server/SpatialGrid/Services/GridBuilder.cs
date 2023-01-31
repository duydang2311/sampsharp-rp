namespace Server.SpatialGrid.Services;

public sealed class GridBuilder : IGridBuilder
{
	public float Top { get; private set; }
	public float Left { get; private set; }
	public float Right { get; private set; }
	public float Bottom { get; private set; }
	public int Rows { get; private set; }
	public int Columns { get; private set; }
	public IDictionary<(int Row, int Column), IGridBuilder> InnerGrids { get; } = new Dictionary<(int Row, int Column), IGridBuilder>();

	public IGrid BuildGrid()
	{
		return Grid.From(this);
	}

	public IGridBuilder SetAsInnerGrid(int row, int column, Func<IGridBuilder, IGridBuilder> gridBuilderOptions)
	{
		var gridBuilder = new GridBuilder();
		InnerGrids.Add((row, column), gridBuilderOptions(gridBuilder));
		return this;
	}

	public IGridBuilder SetColumns(int columns)
	{
		Columns = columns;
		return this;
	}

	public IGridBuilder SetBottom(float bottom)
	{
		Bottom = bottom;
		return this;
	}

	public IGridBuilder SetLeft(float left)
	{
		Left = left;
		return this;
	}

	public IGridBuilder SetRows(int rows)
	{
		Rows = rows;
		return this;
	}

	public IGridBuilder SetTop(float top)
	{
		Top = top;
		return this;
	}

	public IGridBuilder SetRight(float right)
	{
		Right = right;
		return this;
	}
}

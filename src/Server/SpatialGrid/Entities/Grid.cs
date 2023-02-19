using SampSharp.Entities.SAMP;
using Server.Geometry.Entities;
using Server.SpatialGrid.Components;
using Server.SpatialGrid.Services;

namespace Server.SpatialGrid.Entities;

public class Grid : BaseCell, IGrid
{
	private readonly IBaseCell[,] cells;
	private readonly float cellWidth;
	private readonly float cellHeight;

	public IRectangleArea Area { get; }
	public int Columns { get; }
	public int Rows { get; }

	public Grid(IGridBuilder builder)
	{
		Columns = builder.Columns;
		Rows = builder.Rows;
		cells = new BaseCell[Rows, Columns];
		Area = new RectangleArea(new Vector2(builder.Left, builder.Top), new Vector2(builder.Right, builder.Bottom));
		cellWidth = (Area.RightBottom.X - Area.LeftTop.X) / Columns;
		cellHeight = (Area.LeftTop.Y - Area.RightBottom.Y) / Rows;
		for (var row = 0; row != Rows; ++row)
		{
			for (var col = 0; col != Columns; ++col)
			{
				cells[row, col] = builder.InnerGrids.TryGetValue((row, col), out var innerBuilder)
					? innerBuilder.BuildGrid()
					: new Cell();
			}
		}
	}

	private LinkedList<(int Row, int Column)> FilterIndex(int row, int col, float radius)
	{
		var cells = new LinkedList<(int, int)>();
		var depth = (int)Math.Ceiling(radius / Math.Min(cellWidth, cellHeight));
		var limit = 1 + depth;
		for (var offRow = -1 * depth; offRow != limit; ++offRow)
		{
			var computedRow = row + offRow;
			if (computedRow < 0 || computedRow >= Rows)
			{
				continue;
			}

			for (var offCol = -1 * depth; offCol != limit; ++offCol)
			{
				var computedCol = col + offCol;
				if (computedCol < 0 || computedCol >= Columns)
				{
					continue;
				}

				cells.AddLast((computedRow, computedCol));
			}
		}
		return cells;
	}

	private LinkedList<(int Row, int Column)> FilterIndex(int row, int col, IArea area)
	{
		var size = new Vector2(area.RightBottom.X - area.LeftTop.X, area.LeftTop.Y - area.RightBottom.Y);
		return FilterIndex(row, col, (float)Math.Sqrt(size.X * size.X + size.Y * size.Y) / 2);
	}

	public override int Add(ISpatialComponent component)
	{
		if (!TryComputeIndex(component.Area, out var row, out var col))
		{
			return 0;
		}

		var indexes = FilterIndex(row, col, component.Area);
		var count = 0;
		foreach (var (Row, Column) in indexes)
		{
			if (component.Area.Overlaps(GetCellArea(Row, Column)))
			{
				count += cells[Row, Column].Add(component);
			}
		}
		return count;
	}

	public override int Remove(ISpatialComponent component)
	{
		if (!TryComputeIndex(component.Area, out var row, out var col))
		{
			return 0;
		}

		var indexes = FilterIndex(row, col, component.Area);
		var count = 0;
		foreach (var (Row, Column) in indexes)
		{
			if (component.Area.Overlaps(GetCellArea(Row, Column)))
			{
				count += cells[Row, Column].Remove(component);
			}
		}
		return count;
	}

	public override void Clear()
	{
		for (var row = 0; row != Rows; ++row)
		{
			for (var col = 0; col != Columns; ++col)
			{
				cells[row, col].Clear();
			}
		}
	}

	public IEnumerable<IBaseCell> FindCells(Vector2 position, float radius)
	{
		if (!TryComputeIndex(position, out var row, out var col))
		{
			return Array.Empty<IBaseCell>();
		}

		return FilterIndex(row, col, radius).Select(i => cells[i.Row, i.Column]);
	}

	public IEnumerable<ISpatialComponent> FindComponents(Vector2 position, float radius)
	{
		if (!TryComputeIndex(position, out var row, out var col))
		{
			return Array.Empty<ISpatialComponent>();
		}

		var components = new LinkedList<ISpatialComponent>();
		var indexes = FilterIndex(row, col, radius);
		foreach (var (Row, Column) in indexes)
		{
			var baseCell = cells[Row, Column];
			if (baseCell is ICell cell)
			{
				foreach (var component in cell.Components)
				{
					components.AddLast(component);
				}
			}
			else if (baseCell is IGrid grid)
			{
				foreach (var component in grid.FindComponents(position, radius))
				{
					components.AddLast(component);
				}
			}
		}
		return components;
	}

	private bool TryComputeIndex(Vector2 position, out int row, out int column)
	{
		if (!Area.Contains(position))
		{
			row = default;
			column = default;
			return false;
		}
		column = (int)((position.X - Area.LeftTop.X) / cellWidth);
		row = (int)((Area.LeftTop.Y - position.Y) / cellHeight);
		return true;
	}

	private bool TryComputeIndex(IArea area, out int row, out int column)
	{
		column = Math.Clamp((int)Math.Round((area.LeftTop.X - Area.LeftTop.X) / cellWidth), 0, Columns);
		row = Math.Clamp((int)Math.Round((Area.LeftTop.Y - area.LeftTop.Y) / cellHeight), 0, Rows);
		return area.Overlaps(GetCellArea(row, column));
	}

	private IRectangleArea GetCellArea(int row, int col)
	{
		var cellTopleft = Area.LeftTop + new Vector2(col * cellWidth, -row * cellHeight);
		return new RectangleArea(cellTopleft, cellTopleft + new Vector2(cellWidth, -cellHeight));
	}
}

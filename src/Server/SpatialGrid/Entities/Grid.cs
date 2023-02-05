using SampSharp.Entities.SAMP;
using Server.SpatialGrid.Components;
using Server.SpatialGrid.Services;

namespace Server.SpatialGrid.Entities;

public class Grid : BaseCell, IGrid
{
	private readonly IBaseCell[,] cells;

	public Vector2 End { get; }
	public int Columns { get; }
	public int Rows { get; }
	public float CellWidth => (End.X - Start.X) / Columns;
	public float CellHeight => (End.Y - Start.Y) / Rows;

	public Grid(IGridBuilder builder) : base(builder.Left, builder.Top)
	{
		Columns = builder.Columns;
		Rows = builder.Rows;
		End = new Vector2(builder.Right, builder.Bottom);
		cells = new BaseCell[Rows, Columns];
		for (var row = 0; row != Rows; ++row)
		{
			for (var col = 0; col != Columns; ++col)
			{
				if (builder.InnerGrids.TryGetValue((row, col), out var innerBuilder))
				{
					cells[row, col] = innerBuilder.BuildGrid();
				}
				else
				{
					var cellTop = Start.Y + (row * CellHeight);
					var cellLeft = Start.X + (col * CellWidth);
					cells[row, col] = new Cell(cellLeft, cellTop);
				}
			}
		}
	}

	private LinkedList<IBaseCell> GetSurroundingCells(int row, int col, float radius)
	{
		var cells = new LinkedList<IBaseCell>();
		var depth = (int)Math.Ceiling(radius / Math.Min(CellWidth, CellHeight));
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

				cells.AddLast(this.cells[computedRow, computedCol]);
			}
		}
		return cells;
	}

	public bool TryComputeIndex(Vector2 position, out int row, out int column)
	{
		if (position.X < Start.X || position.X >= End.X || position.Y < Start.Y || position.Y >= End.Y)
		{
			row = default;
			column = default;
			return false;
		}
		column = (int)((position.X - Start.X) / CellWidth);
		row = (int)((position.Y - Start.Y) / CellHeight);
		return true;
	}

	public override bool Add(ISpatialComponent component)
	{
		if (!TryComputeIndex(component.Position.XY, out var row, out var col))
		{
			return false;
		}

		var baseCells = GetSurroundingCells(row, col, component.Radius);
		foreach (var baseCell in baseCells)
		{
			if (!TryComputeIndex(baseCell.Start, out row, out col)
			|| !IsIntersect(component, cells[row, col]))
			{
				continue;
			}
			if (baseCell is ICell cell)
			{
				cell.Add(component);
			}
			if (baseCell is IGrid grid)
			{
				grid.Add(component);
			}
		}
		return baseCells.Any();
	}

	public override bool Remove(ISpatialComponent component)
	{
		if (!TryComputeIndex(component.Position.XY, out var row, out var col))
		{
			return false;
		}

		var baseCells = GetSurroundingCells(row, col, component.Radius);
		foreach (var baseCell in baseCells)
		{
			if (!TryComputeIndex(baseCell.Start, out row, out col)
			|| !IsIntersect(component, cells[row, col]))
			{
				continue;
			}
			if (baseCell is ICell cell)
			{
				cell.Remove(component);
			}
			if (baseCell is IGrid grid)
			{
				grid.Remove(component);
			}
		}
		return baseCells.Any();
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

	private bool IsIntersect(ISpatialComponent component, IBaseCell cell)
	{
		var width = CellWidth;
		var height = CellHeight;
		var dx = Math.Abs(component.Position.X - cell.Start.X);
		var dy = Math.Abs(component.Position.Y - cell.Start.Y);
		if (dx > ((width / 2) + component.Radius)
		|| dy > ((height / 2) + component.Radius))
		{
			return false;
		}
		return dx <= width / 2 || dy <= height / 2;
	}

	public IEnumerable<IBaseCell> FindCells(Vector2 position, float radius)
	{
		if (!TryComputeIndex(position, out var row, out var col))
		{
			return Array.Empty<IBaseCell>();
		}

		return GetSurroundingCells(row, col, radius);
	}

	public IEnumerable<ISpatialComponent> FindComponents(Vector2 position, float radius)
	{
		if (!TryComputeIndex(position, out var row, out var col))
		{
			return Array.Empty<ISpatialComponent>();
		}

		var components = new LinkedList<ISpatialComponent>();
		var baseCells = GetSurroundingCells(row, col, radius);
		foreach (var baseCell in baseCells)
		{
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
}

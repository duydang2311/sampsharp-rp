﻿using SampSharp.Entities.SAMP;
using Server.SpatialGrid.Components;
using Server.SpatialGrid.Services;

namespace Server.SpatialGrid.Entities;

public sealed class Grid : BaseCell, IGrid
{
	private readonly IBaseCell[,] cells;

	public float Top { get; set; }
	public float Left { get; set; }
	public float Right { get; set; }
	public float Bottom { get; set; }
	public int Columns { get; }
	public int Rows { get; }
	public float CellWidth => (Right - Left) / Columns;
	public float CellHeight => (Bottom - Top) / Rows;

	public static IGrid From(IGridBuilder builder)
	{
		var grid = new Grid(builder.Rows, builder.Columns)
		{
			Top = builder.Top,
			Left = builder.Left,
			Right = builder.Right,
			Bottom = builder.Bottom,
		};
		for (var row = 0; row != grid.Rows; ++row)
		{
			for (var col = 0; col != grid.Columns; ++col)
			{
				if (builder.InnerGrids.TryGetValue((row, col), out var innerBuilder))
				{
					var top = grid.Top + (row * grid.CellHeight);
					var left = grid.Left + (col * grid.CellWidth);
					grid.cells[row, col] = innerBuilder
						.SetTop(top)
						.SetLeft(left)
						.SetRight(left + grid.CellWidth)
						.SetBottom(top + grid.CellHeight)
						.BuildGrid();
				}
				else
				{
					grid.cells[row, col] = new Cell();
				}
			}
		}
		return grid;
	}

	private Grid(int rows, int columns)
	{
		Columns = columns;
		Rows = rows;
		cells = new BaseCell[rows, columns];
	}

	private IEnumerable<IBaseCell> GetSurroundingCells(int row, int col, int depth)
	{
		var cells = new LinkedList<IBaseCell>();
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

	public IEnumerable<IBaseCell> GetSurroundingCells(BaseSpatialComponent component)
	{
		if (!TryComputeIndex(component.Position, out var row, out var col))
		{
			return Array.Empty<IBaseCell>();
		}

		var depth = (int)Math.Ceiling(component.Range / Math.Min(CellWidth, CellHeight));
		return GetSurroundingCells(row, col, depth);
	}

	public bool TryComputeIndex(Vector2 position, out int row, out int column)
	{
		if (position.X < Left || position.X >= Right || position.Y < Top || position.Y >= Bottom)
		{
			row = default;
			column = default;
			return false;
		}
		column = (int)((position.X - Left) / CellWidth);
		row = (int)((position.Y - Top) / CellHeight);
		return true;
	}

	public IBaseCell? FindCell(Vector2 position)
	{
		return !TryComputeIndex(position, out var row, out var column) ? default : cells[row, column];
	}

	public IBaseCell? FindCell(Predicate<IBaseCell> cellPredicate)
	{
		for (var row = 0; row != Rows; ++row)
		{
			for (var col = 0; col != Columns; ++col)
			{
				if (cellPredicate(cells[row, col]))
				{
					return cells[row, col];
				}
			}
		}
		return default;
	}

	public override bool Add(BaseSpatialComponent component)
	{
		if (!TryComputeIndex(component.Position, out var row, out var col))
		{
			return false;
		}
		var baseCell = cells[row, col];
		if (baseCell is ICell cell)
		{
			cell.Add(component);
			return true;
		}
		if (baseCell is IGrid grid)
		{
			return grid.Add(component);
		}
		return false;
	}

	public override bool Remove(BaseSpatialComponent component)
	{
		if (!TryComputeIndex(component.Position, out var row, out var col))
		{
			return false;
		}
		var baseCell = cells[row, col];
		if (baseCell is ICell cell)
		{
			return cell.Remove(component);
		}
		if (baseCell is IGrid grid)
		{
			return grid.Remove(component);
		}
		return false;
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
}

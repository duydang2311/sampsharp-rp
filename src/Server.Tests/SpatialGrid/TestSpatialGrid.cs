using FluentAssertions;
using FluentAssertions.Execution;
using SampSharp.Entities.SAMP;
using Server.SpatialGrid.Entities;
using Server.SpatialGrid.Services;

namespace Server.Tests.SpatialGrid;

public class TestSpatialGrid
{
	[Test]
	public void Grid_HasCorrectCellSize()
	{
		var grid = new GridBuilder()
			.SetTop(-3000)
			.SetLeft(-3000)
			.SetRight(3000)
			.SetBottom(3000)
			.SetRows(12)
			.SetColumns(10)
			.BuildGrid();
		using (new AssertionScope())
		{
			grid.CellWidth.Should().Be(6000 / 10);
			grid.CellHeight.Should().Be(6000 / 12);
		}
	}

	[Test]
	public void Grid_ComputeCorrectly()
	{
		var grid = new GridBuilder()
			.SetTop(-3000)
			.SetLeft(-3000)
			.SetRight(3000)
			.SetBottom(3000)
			.SetRows(10)
			.SetColumns(3)
			.BuildGrid();
		using (new AssertionScope())
		{
			var ok = grid.TryComputeIndex(new Vector2(-3000, -2400), out var row, out var col);
			ok.Should().BeTrue();
			row.Should().Be(1);
			col.Should().Be(0);

			ok = grid.TryComputeIndex(new Vector2(-1000, -1800), out row, out col);
			ok.Should().BeTrue();
			row.Should().Be(2);
			col.Should().Be(1);

			ok = grid.TryComputeIndex(new Vector2(2999, 2999), out row, out col);
			ok.Should().BeTrue();
			row.Should().Be(9);
			col.Should().Be(2);
		}
	}

	[Test]
	public void NestedGrid_HasCorrectCellSize()
	{
		var grid = new GridBuilder()
			.SetTop(-3000)
			.SetLeft(-3000)
			.SetRight(3000)
			.SetBottom(3000)
			.SetRows(12)
			.SetColumns(10)
			.SetAsInnerGrid(0, 0, innerGrid => innerGrid
				.SetRows(4)
				.SetColumns(4))
			.BuildGrid();
		using (new AssertionScope())
		{
			grid.FindCells(new Vector2(-3000, -3000), 0).First().Should().BeAssignableTo<IGrid>();
			(grid.FindCells(new Vector2(-3000, -3000), 0).First() as IGrid)!.CellWidth.Should().Be(6000 / 10 / 4);
			(grid.FindCells(new Vector2(-3000, -3000), 0).First() as IGrid)!.CellHeight.Should().Be(6000 / 12 / 4);
		}
	}

	[Test]
	public void NestedGrid_ComputeCorrectly()
	{
		var grid = new GridBuilder()
			.SetTop(-3000)
			.SetLeft(-3000)
			.SetRight(3000)
			.SetBottom(3000)
			.SetRows(12)
			.SetColumns(10)
			.SetAsInnerGrid(0, 0, innerGrid => innerGrid
				.SetRows(4)
				.SetColumns(4))
			.SetAsInnerGrid(11, 9, innerGrid => innerGrid
				.SetRows(4)
				.SetColumns(4))
			.BuildGrid();
		using (new AssertionScope())
		{
			var innerGrid1 = (IGrid)grid.FindCells(new Vector2(-3000, -3000), 0).First();
			var innerGrid2 = (IGrid)grid.FindCells(new Vector2(2400, 2500), 0).First();

			var ok = innerGrid1.TryComputeIndex(new Vector2(-3000, -3000), out var row, out var col);
			ok.Should().BeTrue();
			row.Should().Be(0);
			col.Should().Be(0);

			ok = innerGrid1.TryComputeIndex(new Vector2(-3000 + 150, -3000 + 250), out row, out col);
			ok.Should().BeTrue();
			row.Should().Be(2);
			col.Should().Be(1);

			ok = innerGrid2.TryComputeIndex(new Vector2(2400, 2500), out row, out col);
			ok.Should().BeTrue();
			row.Should().Be(0);
			col.Should().Be(0);

			ok = innerGrid2.TryComputeIndex(new Vector2(2400 + 300, 2500 + 375), out row, out col);
			ok.Should().BeTrue();
			row.Should().Be(3);
			col.Should().Be(2);

			ok = innerGrid2.TryComputeIndex(new Vector2(2999, 2999), out row, out col);
			ok.Should().BeTrue();
			row.Should().Be(3);
			col.Should().Be(3);
		}
	}
}

using SampSharp.Entities.SAMP;
using Server.SpatialGrid;

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
		Assert.Multiple(() =>
		{
			Assert.That(grid.CellWidth, Is.EqualTo(6000 / 10));
			Assert.That(grid.CellHeight, Is.EqualTo(6000 / 12));
		});
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
		Assert.Multiple(() =>
		{
			var ok = grid.TryComputeIndex(new Vector2(-3000, -2400), out var row, out var col);
			Assert.That(ok, Is.EqualTo(true));
			Assert.That(row, Is.EqualTo(1));
			Assert.That(col, Is.EqualTo(0));

			ok = grid.TryComputeIndex(new Vector2(-1000, -1800), out row, out col);
			Assert.That(ok, Is.EqualTo(true));
			Assert.That(row, Is.EqualTo(2));
			Assert.That(col, Is.EqualTo(1));

			ok = grid.TryComputeIndex(new Vector2(2999, 2999), out row, out col);
			Assert.That(ok, Is.EqualTo(true));
			Assert.That(row, Is.EqualTo(9));
			Assert.That(col, Is.EqualTo(2));
		});
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
		Assert.Multiple(() =>
		{
			Assert.That(grid.FindCell(new Vector2(-3000, -3000)), Is.InstanceOf(typeof(IGrid)));
			Assert.That((grid.FindCell(new Vector2(-3000, -3000)) as IGrid)!.CellWidth, Is.EqualTo(6000 / 10 / 4));
			Assert.That((grid.FindCell(new Vector2(-3000, -3000)) as IGrid)!.CellHeight, Is.EqualTo(6000 / 12 / 4));
		});
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
		Assert.Multiple(() =>
		{
			var innerGrid1 = (IGrid)grid.FindCell(new Vector2(-3000, -3000))!;
			var innerGrid2 = (IGrid)grid.FindCell(new Vector2(2400, 2500))!;

			var ok = innerGrid1.TryComputeIndex(new Vector2(-3000, -3000), out var row, out var col);
			Assert.That(ok, Is.EqualTo(true));
			Assert.That(row, Is.EqualTo(0));
			Assert.That(col, Is.EqualTo(0));

			ok = innerGrid1.TryComputeIndex(new Vector2(-3000 + 150, -3000 + 250), out row, out col);
			Assert.That(ok, Is.EqualTo(true));
			Assert.That(row, Is.EqualTo(2));
			Assert.That(col, Is.EqualTo(1));

			ok = innerGrid2.TryComputeIndex(new Vector2(2400, 2500), out row, out col);
			Assert.That(ok, Is.EqualTo(true));
			Assert.That(row, Is.EqualTo(0));
			Assert.That(col, Is.EqualTo(0));

			ok = innerGrid2.TryComputeIndex(new Vector2(2400 + 300, 2500 + 375), out row, out col);
			Assert.That(ok, Is.EqualTo(true));
			Assert.That(row, Is.EqualTo(3));
			Assert.That(col, Is.EqualTo(2));

			ok = innerGrid2.TryComputeIndex(new Vector2(2999, 2999), out row, out col);
			Assert.That(ok, Is.EqualTo(true));
			Assert.That(row, Is.EqualTo(3));
			Assert.That(col, Is.EqualTo(3));
		});
	}
}

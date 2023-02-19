using FluentAssertions;
using FluentAssertions.Execution;
using Server.Geometry.Entities;
using Server.SpatialGrid.Components;
using Server.SpatialGrid.Entities;
using Server.SpatialGrid.Services;

namespace Server.Tests.SpatialGrid;

public class TestSpatialGrid
{
	private IGrid grid = null!;

	[SetUp]
	public void SetUp()
	{
		grid = new GridBuilder()
			.SetTop(3000)
			.SetLeft(-3000)
			.SetRight(3000)
			.SetBottom(-3000)
			.SetRows(12)
			.SetColumns(10)
			.SetAsInnerGrid(0, 0, innerGrid => innerGrid
				.SetRows(4)
				.SetColumns(4))
			.SetAsInnerGrid(11, 9, innerGrid => innerGrid
				.SetRows(4)
				.SetColumns(4))
			.BuildGrid();
	}

	[Test]
	public void Grid_AddAndRemove_ReturnsCorrectCellCount()
	{
		var smallComponent = new TestComponent(new CircleArea(-2400, 2500) { Radius = 1f });
		var mediumComponent = new TestComponent(new CircleArea(-2400, 2500) { Radius = 600f });
		var largeComponent = new TestComponent(new CircleArea(-6000, 6000) { Radius = 6000f });
		using (new AssertionScope())
		{
			grid.Add(smallComponent).Should().Be(3);
			grid.Add(mediumComponent).Should().Be(10);
			grid.Add(largeComponent).Should().Be(150);
			grid.Remove(smallComponent).Should().Be(3);
			grid.Remove(mediumComponent).Should().Be(10);
			grid.Remove(largeComponent).Should().Be(150);
		}
	}

	private class TestComponent : BaseSpatialComponent
	{
		public TestComponent(IArea area) : base(area) { }
	}
}

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using SampSharp.Entities.SAMP;
using Server.Geometry.Entities;
using Server.SpatialGrid.Components;
using Server.SpatialGrid.Entities;
using Server.SpatialGrid.Services;

namespace Server.Tests.SpatialGrid;

[Config(typeof(Config))]
public class BenchmarkSpatialGrid
{
	private class Config : ManualConfig
	{
		public Config()
		{
			AddJob(Job.ShortRun);
			WithOptions(ConfigOptions.DisableOptimizationsValidator);
		}
	}

	private readonly IGrid grid;
	private readonly Consumer consumer = new();

	public BenchmarkSpatialGrid()
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

	[Benchmark]
	public void FindCells()
	{
		grid.FindCells(new Vector2(-3000, -3000), 0).Consume(consumer);
	}

	[Benchmark]
	public void AddComponent()
	{
		grid.Add(new TestComponent(new CircleArea(-2400, -2500) { Radius = 300f }));
	}

	private class TestComponent : BaseSpatialComponent
	{
		public TestComponent(IArea area) : base(area) { }
	}
}

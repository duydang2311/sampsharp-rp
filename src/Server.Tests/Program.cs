using BenchmarkDotNet.Running;
using Server.Tests.SpatialGrid;

namespace Server.Tests;

public class Program
{
	public static void Main()
	{
		BenchmarkRunner.Run<BenchmarkSpatialGrid>();
	}
}

namespace Server.SpatialGrid.Services;

public sealed class GridBuiderFactory : IGridBuilderFactory
{
	public IGridBuilder CreateGridBuilder()
	{
		return new GridBuilder();
	}
}

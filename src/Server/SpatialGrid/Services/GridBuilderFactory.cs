namespace Server.SpatialGrid.Services;

public sealed class GridBuiderFactory : IGridBuilderFactory
{
	public IGridBuilder CreateBuilder()
	{
		return new GridBuilder();
	}
}

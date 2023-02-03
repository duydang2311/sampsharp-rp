using Server.SpatialGrid.Services;

namespace Server.SpatialGrid.Entities;

public sealed class SanAndreasGrid : Grid, ISanAndreasGrid
{
	public SanAndreasGrid(IGridBuilderFactory factory)
	: base(factory
		.CreateGridBuilder()
		.SetTop(-3000)
		.SetLeft(-3000)
		.SetRight(3000)
		.SetBottom(3000)
		.SetRows(4)
		.SetColumns(4))
	{ }
}

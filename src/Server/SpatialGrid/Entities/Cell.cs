using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public sealed class Cell : BaseCell, ICell
{
	private readonly LinkedList<BaseSpatialComponent> components = new();
	public IEnumerable<BaseSpatialComponent> Components => components;

	public Cell(float x, float y) : base(x, y) { }

	public override bool Add(BaseSpatialComponent component)
	{
		components.AddLast(component);
		return true;
	}

	public override void Clear()
	{
		components.Clear();
	}

	public override bool Remove(BaseSpatialComponent component)
	{
		return components.Remove(component);
	}
}

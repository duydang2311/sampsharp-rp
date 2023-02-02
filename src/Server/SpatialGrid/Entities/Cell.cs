using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public sealed class Cell : BaseCell, ICell
{
	private readonly LinkedList<BaseSpatialComponent> components = new();
	public IEnumerable<BaseSpatialComponent> Components => components;

	public void Add(BaseSpatialComponent component)
	{
		components.AddLast(component);
	}

	public void Clear()
	{
		components.Clear();
	}

	public bool Remove(BaseSpatialComponent component)
	{
		return components.Remove(component);
	}
}

using SampSharp.Entities;

namespace Server.SpatialGrid.Entities;

public sealed class Cell : BaseCell, ICell
{
	private readonly LinkedList<Component> components = new();
	public IEnumerable<Component> Components => components;

	public void Add(Component component)
	{
		components.AddLast(component);
	}

	public void Clear()
	{
		components.Clear();
	}

	public bool Remove(Component component)
	{
		return components.Remove(component);
	}
}

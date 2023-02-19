using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public sealed class Cell : BaseCell, ICell
{
	private readonly LinkedList<ISpatialComponent> components = new();
	public IReadOnlyCollection<ISpatialComponent> Components => components;

	public Cell() { }

	public override int Add(ISpatialComponent component)
	{
		components.AddLast(component);
		return 1;
	}

	public override int Remove(ISpatialComponent component)
	{
		components.Remove(component);
		return 1;
	}

	public override void Clear()
	{
		components.Clear();
	}
}

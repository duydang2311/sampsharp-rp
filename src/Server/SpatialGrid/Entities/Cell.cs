using Server.SpatialGrid.Components;

namespace Server.SpatialGrid.Entities;

public sealed class Cell : BaseCell, ICell
{
	private readonly LinkedList<ISpatialComponent> components = new();
	public IEnumerable<ISpatialComponent> Components => components;

	public Cell(float x, float y) : base(x, y) { }

	public override bool Add(ISpatialComponent component)
	{
		components.AddLast(component);
		return true;
	}

	public override void Clear()
	{
		components.Clear();
	}

	public override bool Remove(ISpatialComponent component)
	{
		return components.Remove(component);
	}
}

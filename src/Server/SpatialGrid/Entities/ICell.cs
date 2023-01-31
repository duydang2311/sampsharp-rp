using SampSharp.Entities;

namespace Server.SpatialGrid.Entities;

public interface ICell : IBaseCell
{
	IEnumerable<Component> Components { get; }
	void Add(Component component);
	bool Remove(Component component);
	void Clear();
}

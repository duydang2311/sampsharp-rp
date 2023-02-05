using Server.SpatialGrid.Components;

namespace Server.Door.Entities;

public abstract class BaseDoor : BaseSpatialComponent, IDoor
{
	public long Id { get; set; }

	public BaseDoor(float x, float y) : base(x, y, 2f) { }
}

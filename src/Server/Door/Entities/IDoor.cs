using Server.SpatialGrid.Components;

namespace Server.Door.Entities;

public interface IDoor : ISpatialComponent
{
	long Id { get; set; }
}

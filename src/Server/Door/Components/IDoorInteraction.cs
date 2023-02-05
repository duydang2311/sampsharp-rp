using Server.Door.Entities;
using Server.SpatialGrid.Components;

namespace Server.Door.Components;

public interface IDoorInteraction : ISpatialComponent
{
	IDoor Door { get; }
	int World { get; set; }
	int Interior { get; set; }
}

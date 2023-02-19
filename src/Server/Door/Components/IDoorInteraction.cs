using Server.Door.Entities;
using Server.SpatialGrid.Components;

namespace Server.Door.Components;

public interface IDoorInteraction : ISpatialComponent
{
	IDoor Door { get; }
	float Z { get; }
	float Angle { get; set; }
	int World { get; set; }
	int Interior { get; set; }
}

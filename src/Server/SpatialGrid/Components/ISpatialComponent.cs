using SampSharp.Entities.SAMP;

namespace Server.SpatialGrid.Components;

public interface ISpatialComponent
{
	Vector3 Position { get; set; }
	float Radius { get; set; }
}

using SampSharp.Entities.SAMP;

namespace Server.SpatialGrid.Components;

public interface ISpatialComponent
{
	Vector2 Position { get; set; }
	float Range { get; set; }
}

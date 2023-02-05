using SampSharp.Entities.SAMP;

namespace Server.SpatialGrid.Components;

public abstract class BaseSpatialComponent : ISpatialComponent
{
	public Vector3 Position { get; set; }
	public float Radius { get; set; }

	public BaseSpatialComponent(float x, float y, float z, float radius)
	{
		Position = new Vector3(x, y, z);
		Radius = radius;
	}
}

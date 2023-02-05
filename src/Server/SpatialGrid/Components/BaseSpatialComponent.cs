using SampSharp.Entities.SAMP;

namespace Server.SpatialGrid.Components;

public abstract class BaseSpatialComponent : ISpatialComponent
{
	public Vector2 Position { get; set; }
	public float Range { get; set; }

	public BaseSpatialComponent(float x, float y, float range)
	{
		Position = new Vector2(x, y);
		Range = range;
	}
}

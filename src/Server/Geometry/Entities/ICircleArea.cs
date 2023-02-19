using SampSharp.Entities.SAMP;

namespace Server.Geometry.Entities;

public interface ICircleArea : IArea
{
	Vector2 Center { get; }
	float Radius { get; }
	float RadiusSquared { get; }
}

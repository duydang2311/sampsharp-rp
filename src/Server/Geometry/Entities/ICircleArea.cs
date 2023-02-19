namespace Server.Geometry.Entities;

public interface ICircleArea : IArea
{
	float Radius { get; set; }
	float RadiusSquared { get; }
}

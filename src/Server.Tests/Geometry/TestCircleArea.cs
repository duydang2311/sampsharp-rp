using FluentAssertions;
using FluentAssertions.Execution;
using SampSharp.Entities.SAMP;
using Server.Geometry.Entities;

namespace Server.Tests.Geometry;

public class TestCircleArea
{
	[Test]
	public void Circle_ContainsPoint()
	{
		IArea circle = new CircleArea() { Center = new Vector2(2, 0), Radius = 2 };
		circle.Contains(new Vector2(0, 0)).Should().BeTrue();
	}

	[Test]
	public void Circle_Not_ContainsPoint()
	{
		IArea circle = new CircleArea() { Center = new Vector2(2, 0), Radius = 2 };
		circle.Contains(new Vector2(0, 1)).Should().BeFalse();
	}

	[Test]
	public void Circle_OverlapsArea()
	{
		IArea circle = new CircleArea() { Center = new Vector2(2, 0), Radius = 2 };
		IArea rectangle1 = new RectangleArea(new Vector2(0, 6), new Vector2(6, 2));
		IArea rectangle2 = new RectangleArea(new Vector2(-100, 100), new Vector2(100, -100));
		IArea rectangle3 = new RectangleArea(new Vector2(1, 2), new Vector2(3, 1));
		IArea rectangle4 = new RectangleArea(new Vector2(4, 6), new Vector2(6, 0));
		using (new AssertionScope())
		{
			circle.Overlaps(rectangle1).Should().BeTrue();
			circle.Overlaps(rectangle2).Should().BeTrue();
			circle.Overlaps(rectangle3).Should().BeTrue();
			circle.Overlaps(rectangle4).Should().BeTrue();
		}
	}

	[Test]
	public void Circle_Not_OverlapsArea()
	{
		IArea circle = new CircleArea() { Center = new Vector2(2, 0), Radius = 2 };
		IArea rectangle1 = new RectangleArea(new Vector2(0, 6), new Vector2(6, 2.00001));
		IArea rectangle2 = new RectangleArea(new Vector2(-100, 100), new Vector2(-0.0001, 0001));
		IArea rectangle3 = new RectangleArea(new Vector2(4.00001, 6), new Vector2(6, 0));
		IArea rectangle4 = new RectangleArea(new Vector2(0, -2.0001), new Vector2(1, -4));
		using (new AssertionScope())
		{
			circle.Overlaps(rectangle1).Should().BeFalse();
			circle.Overlaps(rectangle2).Should().BeFalse();
			circle.Overlaps(rectangle3).Should().BeFalse();
			circle.Overlaps(rectangle4).Should().BeFalse();
		}
	}
}

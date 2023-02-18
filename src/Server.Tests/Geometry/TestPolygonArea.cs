using SampSharp.Entities.SAMP;
using Server.Geometry.Entities;

namespace Server.Tests.Geometry;

public class TestPolygonArea
{
	IPolygonArea polygon = null!;

	[SetUp]
	public void SetUp()
	{
		polygon = new PolygonArea();
		polygon.Add(new Vector2(0, 0));
		polygon.Add(new Vector2(5, 0));
		polygon.Add(new Vector2(5, 5));
		polygon.Add(new Vector2(2.5, 2.5));
		polygon.Add(new Vector2(0, 7.5));
	}
	[Test]
	public void Polygon_ContainsPoint()
	{
		Assert.Multiple(() =>
		{
			Assert.That(polygon.Contains(new Vector2(0, 0)), Is.True);
			Assert.That(polygon.Contains(new Vector2(4.99, 0)), Is.True);
			Assert.That(polygon.Contains(new Vector2(4.99, 4.99)), Is.True);
			Assert.That(polygon.Contains(new Vector2(2.499, 2.499)), Is.True);
			Assert.That(polygon.Contains(new Vector2(0, 7.499)), Is.True);
			Assert.That(polygon.Contains(new Vector2(1, 1)), Is.True);
			Assert.That(polygon.Contains(new Vector2(4, 4)), Is.True);
			Assert.That(polygon.Contains(new Vector2(3, 3)), Is.True);
			Assert.That(polygon.Contains(new Vector2(1, 5)), Is.True);
		});
	}

	[Test]
	public void Polygon_Not_ContainsPoint()
	{
		Assert.Multiple(() =>
		{
			Assert.That(polygon.Contains(new Vector2(-0.001, -0.001)), Is.False);
			Assert.That(polygon.Contains(new Vector2(5.001, 0)), Is.False);
			Assert.That(polygon.Contains(new Vector2(5.001, 5.001)), Is.False);
			Assert.That(polygon.Contains(new Vector2(2.5, 2.51)), Is.False);
			Assert.That(polygon.Contains(new Vector2(0, 7.5001)), Is.False);
		});
	}

	[Test]
	public void Polygon_OverlapsArea()
	{
		IArea rectangle1 = new RectangleArea(new Vector2(4.99, 4.99), new Vector2(7, 0));
		IArea rectangle2 = new RectangleArea(new Vector2(-100, 100), new Vector2(0.001, 0.001));
		IArea rectangle3 = new RectangleArea(new Vector2(0, 10), new Vector2(2, 7.499));
		IArea rectangle4 = new RectangleArea(new Vector2(2.4, 2.6), new Vector2(2.6, 2.4));
		IArea circle1 = new CircleArea() { Center = new Vector2(-2, 0), Radius = 2 };
		IArea circle2 = new CircleArea() { Center = new Vector2(0, 9.5), Radius = 2 };
		IArea circle3 = new CircleArea() { Center = new Vector2(7, 0), Radius = 2 };
		IArea circle4 = new CircleArea() { Center = new Vector2(2.5, 5.5), Radius = 2 };
		Assert.Multiple(() =>
		{
			Assert.That(polygon.Overlaps(rectangle1), Is.True);
			Assert.That(polygon.Overlaps(rectangle2), Is.True);
			Assert.That(polygon.Overlaps(rectangle3), Is.True);
			Assert.That(polygon.Overlaps(rectangle4), Is.True);
			Assert.That(polygon.Overlaps(circle1), Is.True);
			Assert.That(polygon.Overlaps(circle2), Is.True);
			Assert.That(polygon.Overlaps(circle3), Is.True);
			Assert.That(polygon.Overlaps(circle4), Is.True);
		});
	}

	[Test]
	public void Polygon_Not_OverlapsArea()
	{
		IArea rectangle1 = new RectangleArea(new Vector2(5.01, 5.01), new Vector2(7, 0));
		IArea rectangle2 = new RectangleArea(new Vector2(-100, 100), new Vector2(-0.001, -0.001));
		IArea rectangle3 = new RectangleArea(new Vector2(0, 10), new Vector2(2, 7.5001));
		IArea rectangle4 = new RectangleArea(new Vector2(2.5, 3), new Vector2(2.51, 2.6));
		IArea circle1 = new CircleArea() { Center = new Vector2(-2, 0), Radius = 1.99f };
		IArea circle2 = new CircleArea() { Center = new Vector2(0, 9.5), Radius = 1.99f };
		IArea circle3 = new CircleArea() { Center = new Vector2(7, 0), Radius = 1.99f };
		IArea circle4 = new CircleArea() { Center = new Vector2(2.5, 7), Radius = 2 };
		Assert.Multiple(() =>
		{
			Assert.That(polygon.Overlaps(rectangle1), Is.False);
			Assert.That(polygon.Overlaps(rectangle2), Is.False);
			Assert.That(polygon.Overlaps(rectangle3), Is.False);
			Assert.That(polygon.Overlaps(rectangle4), Is.False);
			Assert.That(polygon.Overlaps(circle1), Is.False);
			Assert.That(polygon.Overlaps(circle2), Is.False);
			Assert.That(polygon.Overlaps(circle3), Is.False);
			Assert.That(polygon.Overlaps(circle4), Is.False);
		});
	}
}

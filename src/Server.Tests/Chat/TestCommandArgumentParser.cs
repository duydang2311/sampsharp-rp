using Server.Chat.Services;

namespace Server.Tests.Chat;

public class TestCommandArgumentParser
{
	private IArgumentParser parser = null!;

	[SetUp]
	public void Setup()
	{
		parser = new CommandArgumentParser();
	}

	public void SampleDelegate(int a, float b, int c, string d, int? e) { }

	[Test]
	public void TryParse_EmptyString_ReturnsFalse()
	{
		Assert.That(parser.TryParse(SampleDelegate, "", out var _), Is.False);
	}
	[Test]
	public void TryParse_FirstOne_ReturnsFalse()
	{
		Assert.That(parser.TryParse(SampleDelegate, "1", out var _), Is.False);
	}
	[Test]
	public void TryParse_FirstTwo_ReturnsFalse()
	{
		Assert.That(parser.TryParse(SampleDelegate, "1 0.5", out var _), Is.False);
	}
	[Test]
	public void TryParse_FirstThree_ReturnsFalse()
	{
		Assert.That(parser.TryParse(SampleDelegate, "1 0.5 1", out var _), Is.False);
	}
	[Test]
	public void TryParse_FirstFour_ReturnsTrue()
	{
		Assert.That(parser.TryParse(SampleDelegate, "1 0.5 1 text", out var _), Is.True);
	}
	[Test]
	public void TryParse_FullFive_ReturnsTrue()
	{
		Assert.That(parser.TryParse(SampleDelegate, "1 0.5 1 text 1", out var _), Is.True);
	}
	[Test]
	public void TryParse_InvalidNumericType_ReturnsFalse()
	{
		Assert.That(parser.TryParse(SampleDelegate, "1 expected_a_float 1 text", out var _), Is.False);
	}
}

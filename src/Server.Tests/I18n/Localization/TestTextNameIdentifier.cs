using Microsoft.Extensions.DependencyInjection;
using Server.I18n.Localization.Services;

namespace Server.Tests.I18n.Localization;

public class TestTextNameIdentifier
{
	private IServiceProvider provider = null!;
	private ITextNameIdentifierService identifier = null!;

	[SetUp]
	public void Setup()
	{
		var serviceCollection = new ServiceCollection().WithLogging().WithI18n((_, _) =>
		{

		});
		provider = serviceCollection.BuildServiceProvider();
		identifier = provider.GetRequiredService<ITextNameIdentifierService>();
	}

	[Test]
	public void Identify_Property_ServerInfoName_Returns_ServerInfoName_AsString()
	{
		Assert.That(identifier.Identify(m => m.Command_NotFound), Is.EqualTo("Command_NotFound"));
	}
	[Test]
	public void Identify_WithNewExpression_Returns_ServerInfoName_AsString()
	{
		Assert.That(identifier.Identify(m => new { m.Command_NotFound }), Is.EqualTo("Command_NotFound"));
	}
	[Test]
	public void Identify_WithEmptyNewExpression_Returns_EmptyString()
	{
		Assert.That(identifier.Identify(m => new { }), Is.Empty);
	}
	[Test]
	public void Identify_WithUnsupportedExpression_Returns_EmptyString()
	{
		Assert.That(identifier.Identify(m => 1), Is.Empty);
	}
}

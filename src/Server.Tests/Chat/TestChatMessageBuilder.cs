using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;

namespace Server.Tests.Chat;

public class TestChatMessageBuilder
{
	private IChatMessageBuilderFactory factory = null!;

	[SetUp]
	public void Setup()
	{
		var provider = new ServiceCollection()
			.WithLogging()
			.WithI18N((globalOptions, localOptions) =>
			{
				globalOptions.AddCulture("vi");
				localOptions.BaseName = "Server.Resources.Text";
				localOptions.DefaultCulture = CultureInfo.GetCultureInfo("vi");
			})
			.WithChat()
			.BuildServiceProvider();
		factory = provider.GetRequiredService<IChatMessageBuilderFactory>();
	}

	[Test]
	public void BuildInline_Returns_IdenticalString()
	{
		var builder = factory.CreateBuilder();
		var texts = builder
			.Add("Line 1:")
			.Inline("same color text")
			.Add("Line 2:")
			.Inline(Color.Red, "different color text")
			.Build(CultureInfo.InvariantCulture)
			.ToArray();
		Assert.Multiple(() =>
		{
			Assert.That(texts[0], Is.EqualTo("{FFFFFF}Line 1: same color text"));
			Assert.That(texts[1], Is.EqualTo("{FFFFFF}Line 2: {FF0000}different color text"));
		});
	}
}

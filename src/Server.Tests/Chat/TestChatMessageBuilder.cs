using System.Globalization;
using FluentAssertions;
using FluentAssertions.Execution;
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
			.Add(Color.White, "Line 1:")
			.Inline(Color.White, "same color text")
			.Add(Color.White, "Line 2:")
			.Inline(Color.Red, "different color text")
			.Build(CultureInfo.InvariantCulture)
			.ToArray();
		using (new AssertionScope())
		{
			texts[0].Should().Be("{FFFFFF}Line 1: same color text");
			texts[1].Should().Be("{FFFFFF}Line 2: {FF0000}different color text");
		}
	}
}

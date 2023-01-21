using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Server.Chat.Services;

namespace Server.Tests.Chat;

public class TestChatMessageBuilder
{
	private IChatMessageBuilderFactory factory = null!;

	[SetUp]
	public void Setup()
	{
		var provider = new ServiceCollection()
			.WithI18N((globalOptions, localOptions) => {
				globalOptions.AddCulture("vi");
				localOptions.BaseName = "Server.Resources.Text";
				localOptions.DefaultCulture = CultureInfo.GetCultureInfo("vi");
			})
			.WithChat()
			.BuildServiceProvider();
		factory = provider.GetRequiredService<IChatMessageBuilderFactory>();
	}

	[Test]
	public void TryParse_EmptyString_ReturnsFalse()
	{
		var builder = factory.CreateBuilder();
		var text = builder
			.Add(m => m.CommandDenied)
			.Inline(m => m.CommandNotFound);
		Assert.That(text, Is.EqualTo(""));
	}
}

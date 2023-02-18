using System.Globalization;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.I18N.Localization.Services;
using Server.SAMP.Dialog.Services;
using Server.Tests.Helper;

namespace Server.Tests.Database;

public class TestDialogBuilder
{
	private IDialogBuilderFactory builderFactory = null!;
	private IServiceProvider provider = null!;
	private ITextNameIdentifierService identifier = null!;
	private ITextLocalizerService localizer = null!;

	[SetUp]
	public void SetUp()
	{
		var serviceCollection = new ServiceCollection()
			.WithLogging()
			.WithI18N((globalOptions, textLocalizerOptions) =>
			{
				textLocalizerOptions.BaseName = "Server.Resources.Text";
				textLocalizerOptions.DefaultCulture = CultureInfo.InvariantCulture;
			});
		provider = serviceCollection.BuildServiceProvider();
		identifier = provider.GetRequiredService<ITextNameIdentifierService>();
		localizer = provider.GetRequiredService<ITextLocalizerService>();
		builderFactory = new DialogBuilderFactory(localizer, identifier, new DialogTextBuilderFactory(localizer, identifier));
	}

	[Test]
	public void Build_MessageDialog_Returns_CorrectOutputs()
	{
		var dialog = builderFactory
			.CreateMessageBuilder()
			.SetCaption("Caption")
			.SetContent("Content")
			.SetButton1("Button1")
			.SetButton2("Button2")
			.Build();
		using (new AssertionScope())
		{
			dialog.Caption.Should().Be("Caption");
			dialog.Content.Should().Be("Content");
			dialog.Button1.Should().Be("Button1");
			dialog.Button2.Should().Be("Button2");
		}
	}

	[Test]
	public void Build_InputDialog_Returns_CorrectOutputs()
	{
		var dialog = builderFactory
			.CreateInputBuilder()
			.SetCaption("Caption")
			.SetContent("Content")
			.SetButton1("Button1")
			.SetButton2("Button2")
			.SetIsPassword(true)
			.Build();
		using (new AssertionScope())
		{
			dialog.Caption.Should().Be("Caption");
			dialog.Content.Should().Be("Content");
			dialog.Button1.Should().Be("Button1");
			dialog.Button2.Should().Be("Button2");
			dialog.IsPassword.Should().BeTrue();
		}
	}
	[Test]
	public void Build_ListDialog_Returns_CorrectOutputs()
	{
		var tag1 = new object();
		var tag2 = new object();
		var dialog = builderFactory
			.CreateListBuilder()
			.SetCaption("Caption")
			.SetButton1("Button1")
			.SetButton2("Button2")
			.AddRow("Row 1")
			.WithTag(tag1)
			.AddRow("Row 2")
			.WithTag(tag2)
			.Build();
		var rows = dialog.Rows.ToArray();
		using (new AssertionScope())
		{
			dialog.Caption.Should().Be("Caption");
			dialog.Button1.Should().Be("Button1");
			dialog.Button2.Should().Be("Button2");
			rows[0].Text.Should().Be("Row 1");
			rows[0].Tag.Should().Be(tag1);
			rows[1].Text.Should().Be("Row 2");
			rows[1].Tag.Should().Be(tag2);
		}
	}
	[Test]
	public void Build_TablistDialog_Returns_CorrectOutputs()
	{
		var tag1 = new object();
		var tag2 = new object();
		var dialog = builderFactory
			.CreateTablistBuilder()
			.SetCaption("Caption")
			.SetButton1("Button1")
			.SetButton2("Button2")
			.AddColumn("Header 1")
			.AddColumn("Header 2")
			.AddRow("Column 1.1", "Column 2.1")
			.WithTag(tag1)
			.AddRow("Column 1.2", "Column 2.2")
			.WithTag(tag2)
			.AddRow("Column 1.3", "Column 2.3")
			.Build();
		var rows = dialog.Rows.ToArray();
		var columns = dialog.Header.Columns.ToArray();
		using (new AssertionScope())
		{
			dialog.Caption.Should().Be("Caption");
			dialog.Button1.Should().Be("Button1");
			dialog.Button2.Should().Be("Button2");
			columns[0].Should().Be("Header 1");
			columns[1].Should().Be("Header 2");
			rows[0].Columns.First().Should().Be("Column 1.1");
			rows[0].Columns.Last().Should().Be("Column 2.1");
			rows[0].Tag.Should().Be(tag1);
			rows[1].Columns.First().Should().Be("Column 1.2");
			rows[1].Columns.Last().Should().Be("Column 2.2");
			rows[1].Tag.Should().Be(tag2);
			rows[2].Columns.First().Should().Be("Column 1.3");
			rows[2].Columns.Last().Should().Be("Column 2.3");
			rows[2].Tag.Should().BeNull();
		}
	}

}

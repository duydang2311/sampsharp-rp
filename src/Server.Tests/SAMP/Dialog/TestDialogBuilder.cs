using System.Globalization;
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
		builderFactory = new DialogBuilderFactory(localizer, identifier, new LocalizedTextBuilderFactory(localizer, identifier));
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
		Assert.Multiple(() =>
		{
			Assert.That(dialog.Caption, Is.EqualTo("Caption"));
			Assert.That(dialog.Content, Is.EqualTo("Content"));
			Assert.That(dialog.Button1, Is.EqualTo("Button1"));
			Assert.That(dialog.Button2, Is.EqualTo("Button2"));
		});
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
		Assert.Multiple(() =>
		{
			Assert.That(dialog.Caption, Is.EqualTo("Caption"));
			Assert.That(dialog.Content, Is.EqualTo("Content"));
			Assert.That(dialog.Button1, Is.EqualTo("Button1"));
			Assert.That(dialog.Button2, Is.EqualTo("Button2"));
			Assert.That(dialog.IsPassword, Is.True);
		});
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
		Assert.Multiple(() =>
		{
			Assert.That(dialog.Caption, Is.EqualTo("Caption"));
			Assert.That(dialog.Button1, Is.EqualTo("Button1"));
			Assert.That(dialog.Button2, Is.EqualTo("Button2"));
			Assert.That(rows[0].Text, Is.EqualTo("Row 1"));
			Assert.That(rows[0].Tag, Is.EqualTo(tag1));
			Assert.That(rows[1].Text, Is.EqualTo("Row 2"));
			Assert.That(rows[1].Tag, Is.EqualTo(tag2));
		});
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
		Assert.Multiple(() =>
		{
			Assert.That(dialog.Caption, Is.EqualTo("Caption"));
			Assert.That(dialog.Button1, Is.EqualTo("Button1"));
			Assert.That(dialog.Button2, Is.EqualTo("Button2"));
			Assert.That(columns[0], Is.EqualTo("Header 1"));
			Assert.That(columns[1], Is.EqualTo("Header 2"));
			Assert.That(rows[0].Columns.First(), Is.EqualTo("Column 1.1"));
			Assert.That(rows[0].Columns.Last(), Is.EqualTo("Column 2.1"));
			Assert.That(rows[0].Tag, Is.EqualTo(tag1));
			Assert.That(rows[1].Columns.First(), Is.EqualTo("Column 1.2"));
			Assert.That(rows[1].Columns.Last(), Is.EqualTo("Column 2.2"));
			Assert.That(rows[1].Tag, Is.EqualTo(tag2));
			Assert.That(rows[2].Columns.First(), Is.EqualTo("Column 1.3"));
			Assert.That(rows[2].Columns.Last(), Is.EqualTo("Column 2.3"));
			Assert.That(rows[2].Tag, Is.Null);
		});
	}

}

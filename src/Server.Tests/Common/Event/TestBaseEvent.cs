using FluentAssertions;
using FluentAssertions.Execution;
using Server.Common.Event;

namespace Server.Tests.Common.Event;

public class TestBaseEvent
{
	private IEvent @parameterlessEvent = null!;

	[SetUp]
	public void Setup()
	{
		@parameterlessEvent = new BaseEvent(new EventInvoker());
	}

	[Test]
	public void Invoke_IncreaseCountTo_3()
	{
		int count = 0;
		@parameterlessEvent.AddHandler(() => { ++count; });
		@parameterlessEvent.AddHandler(() => { ++count; });
		@parameterlessEvent.AddHandler(() => { ++count; });
		@parameterlessEvent.Invoke();
		count.Should().Be(3);
	}
	[Test]
	public async Task InvokeAsync_IncreaseCountTo_3_After_1s()
	{
		int count = 0;
		var now = DateTime.Now;
		@parameterlessEvent.AddHandler(async () => { await Task.Delay(1000); ++count; });
		@parameterlessEvent.AddHandler(async () => { await Task.Delay(1000); ++count; });
		@parameterlessEvent.AddHandler(() => { ++count; });
		var task = @parameterlessEvent.InvokeAsync();
		using (new AssertionScope())
		{
			count.Should().Be(1);
			await task;
			count.Should().Be(3);
			(DateTime.Now - now).TotalMilliseconds.Should().BeInRange(900, 1100);
		}
	}

	[Test]
	public async Task InvokeAsyncSerial_IncreaseCountTo_3_After2s()
	{
		int count = 0;
		var now = DateTime.Now;
		@parameterlessEvent.AddHandler(async () => { await Task.Delay(1000); ++count; });
		@parameterlessEvent.AddHandler(async () => { await Task.Delay(1000); ++count; });
		@parameterlessEvent.AddHandler(() => { ++count; });
		var task = @parameterlessEvent.InvokeAsyncSerial();
		using (new AssertionScope())
		{
			count.Should().Be(1);
			await task;
			count.Should().Be(3);
			(DateTime.Now - now).TotalMilliseconds.Should().BeInRange(1900, 2100);
		}
	}
}

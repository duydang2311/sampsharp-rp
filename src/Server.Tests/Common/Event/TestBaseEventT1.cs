using FluentAssertions;
using FluentAssertions.Execution;
using Server.Common.Event;

namespace Server.Tests.Common.Event;

public class TestBaseEventT1
{
	private IEvent<int> @event = null!;

	[SetUp]
	public void Setup()
	{
		@event = new BaseEvent<int>(new EventInvoker());
	}

	[Test]
	public void Invoke_IncreaseCountTo_9()
	{
		int count = 0;
		@event.AddHandler(v => { count += v; });
		@event.AddHandler(v => { count += v; });
		@event.AddHandler(v => { count += v; });
		@event.Invoke(3);
		count.Should().Be(9);
	}
	[Test]
	public async Task InvokeAsync_IncreaseCountTo_9_After_1s()
	{
		int count = 0;
		var now = DateTime.Now;
		@event.AddHandler(async v => { await Task.Delay(1000); count += v; });
		@event.AddHandler(async v => { await Task.Delay(1000); count += v; });
		@event.AddHandler(v => { count += v; });
		var task = @event.InvokeAsync(3);
		using (new AssertionScope())
		{
			count.Should().Be(3);
			await task;
			count.Should().Be(9);
			(DateTime.Now - now).TotalMilliseconds.Should().BeInRange(900, 1100);
		}
	}

	[Test]
	public async Task InvokeAsyncSerial_IncreaseCountTo_9_After2s()
	{
		int count = 0;
		var now = DateTime.Now;
		@event.AddHandler(async v => { await Task.Delay(1000); count += v; });
		@event.AddHandler(async v => { await Task.Delay(1000); count += v; });
		@event.AddHandler(v => { count += v; });
		var task = @event.InvokeAsyncSerial(3);
		using (new AssertionScope())
		{
			count.Should().Be(3);
			await task;
			count.Should().Be(9);
			(DateTime.Now - now).TotalMilliseconds.Should().BeInRange(1900, 2100);
		}
	}
}

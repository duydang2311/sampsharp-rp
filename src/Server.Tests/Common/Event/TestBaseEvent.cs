using Server.Common.Event;

namespace Server.Tests.Common.Event;

public class TestBaseEvent
{
	private IEvent @parameterlessEvent = null!;

	[SetUp]
	public void Setup()
	{
		@parameterlessEvent = new BaseEvent();
	}

	[Test]
	public void Invoke_IncreaseCountTo_3()
	{
		int count = 0;
		@parameterlessEvent.AddHandler(() => { ++count; });
		@parameterlessEvent.AddHandler(() => { ++count; });
		@parameterlessEvent.AddHandler(() => { ++count; });
		@parameterlessEvent.Invoke();
		Assert.That(count, Is.EqualTo(3));
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
		Assert.That(count, Is.EqualTo(1));
		await task;
		Assert.That(count, Is.EqualTo(3));
		Assert.That((DateTime.Now - now).TotalMilliseconds, Is.GreaterThanOrEqualTo(1000 - 10));
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
		Assert.That(count, Is.EqualTo(1));
		await task;
		Assert.That(count, Is.EqualTo(3));
		Assert.That((DateTime.Now - now).TotalMilliseconds, Is.GreaterThanOrEqualTo(2000 - 10));
	}
}

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
		Assert.That(count, Is.EqualTo(9));
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
		Assert.That(count, Is.EqualTo(3));
		await task;
		Assert.Multiple(() =>
		{
			Assert.That(count, Is.EqualTo(9));
			Assert.That((DateTime.Now - now).TotalMilliseconds, Is.GreaterThanOrEqualTo(1000 - 100));
		});
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
		Assert.That(count, Is.EqualTo(3));
		await task;
		Assert.Multiple(() =>
		{
			Assert.That(count, Is.EqualTo(9));
			Assert.That((DateTime.Now - now).TotalMilliseconds, Is.GreaterThanOrEqualTo(2000 - 100));
		});
	}
}

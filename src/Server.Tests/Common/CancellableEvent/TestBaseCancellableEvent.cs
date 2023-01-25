using Server.Common.CancellableEvent;

namespace Server.Tests.Common.CancellableEvent;

public class TestBaseCancellableEvent
{
	private ICancellableEvent parameterlessEvent = null!;

	[SetUp]
	public void Setup()
	{
		parameterlessEvent = new BaseCancellableEvent(new CancellableEventInvoker());
	}

	[Test]
	public void Invoke_IncreaseCountTo_1()
	{
		var count = 0;
		parameterlessEvent.AddHandler((e) => { ++count; e.Cancel = true; });
		parameterlessEvent.AddHandler((e) => { ++count; });
		parameterlessEvent.AddHandler((e) => { ++count; });
		parameterlessEvent.Invoke();
		Assert.That(count, Is.EqualTo(1));
	}

	[Test]
	public void Invoke_IncreaseCountTo_2()
	{
		var count = 0;
		parameterlessEvent.AddHandler((e) => { ++count; });
		parameterlessEvent.AddHandler((e) => { ++count; e.Cancel = true; });
		parameterlessEvent.AddHandler((e) => { ++count; });
		parameterlessEvent.Invoke();
		Assert.That(count, Is.EqualTo(2));
	}
}

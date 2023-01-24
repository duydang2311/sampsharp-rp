namespace Server.Common.Event;

public interface IEventInvoker
{
	void Invoke(Delegate @delegate, params object?[]? args);
	Task InvokeAsync(Delegate @delegate, params object?[]? args);
	Task InvokeAsyncSerial(Delegate @delegate, params object?[]? args);
}

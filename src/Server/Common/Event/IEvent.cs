namespace Server.Common.Event;

public interface IEvent
{
	void AddHandler(Action handler);
	void AddHandler(Func<Task> handler);
	void RemoveHandler(Action handler);
	void RemoveHandler(Func<Task> handler);
	void Invoke();
	Task InvokeAsync();
	Task InvokeAsyncSerial();
}

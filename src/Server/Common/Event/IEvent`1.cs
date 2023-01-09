namespace Server.Common.Event;

public interface IEvent<T1>
{
	void AddHandler(Action<T1> handler);
	void AddHandler(Func<T1, Task> handler);
	void RemoveHandler(Action<T1> handler);
	void RemoveHandler(Func<T1, Task> handler);
	void Invoke(T1 arg1);
	Task InvokeAsync(T1 arg1);
	Task InvokeAsyncSerial(T1 arg1);
}

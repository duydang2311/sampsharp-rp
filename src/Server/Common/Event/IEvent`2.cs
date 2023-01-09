namespace Server.Common.Event;

public interface IEvent<T1, T2>
{
	void AddHandler(Action<T1, T2> handler);
	void AddHandler(Func<T1, T2, Task> handler);
	void RemoveHandler(Action<T1, T2> handler);
	void RemoveHandler(Func<T1, T2, Task> handler);
	void Invoke(T1 arg1, T2 arg2);
	Task InvokeAsync(T1 arg1, T2 arg2);
	Task InvokeAsyncSerial(T1 arg1, T2 arg2);
}

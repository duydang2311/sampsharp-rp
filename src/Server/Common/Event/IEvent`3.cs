namespace Server.Common.Event;

public interface IEvent<T1, T2, T3>
{
	void AddHandler(Action<T1, T2, T3> handler);
	void AddHandler(Func<T1, T2, T3, Task> handler);
	void RemoveHandler(Action<T1, T2, T3> handler);
	void RemoveHandler(Func<T1, T2, T3, Task> handler);
	void Invoke(T1 arg1, T2 arg2, T3 arg3);
	Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3);
	Task InvokeAsyncSerial(T1 arg1, T2 arg2, T3 arg3);
}

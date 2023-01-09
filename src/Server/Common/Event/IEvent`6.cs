namespace Server.Common.Event;

public interface IEvent<T1, T2, T3, T4, T5, T6>
{
	void AddHandler(Action<T1, T2, T3, T4, T5, T6> handler);
	void AddHandler(Func<T1, T2, T3, T4, T5, T6, Task> handler);
	void RemoveHandler(Action<T1, T2, T3, T4, T5, T6> handler);
	void RemoveHandler(Func<T1, T2, T3, T4, T5, T6, Task> handler);
	void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	Task InvokeAsyncSerial(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
}

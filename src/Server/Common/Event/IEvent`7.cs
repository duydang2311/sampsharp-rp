namespace Server.Common.Event;

public interface IEvent<T1, T2, T3, T4, T5, T6, T7>
{
    void AddHandler(Action<T1, T2, T3, T4, T5, T6, T7> handler);
    void AddHandler(Func<T1, T2, T3, T4, T5, T6, T7, Task> handler);
    void RemoveHandler(Action<T1, T2, T3, T4, T5, T6, T7> handler);
    void RemoveHandler(Func<T1, T2, T3, T4, T5, T6, T7, Task> handler);
    void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    Task InvokeAsyncSerial(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
}

namespace Server.Chat.Services;

public interface IArgumentParser
{
	bool TryParse(Delegate @delegate, string input, out object?[]? arguments);
	bool TryParse<T1>(Delegate @delegate, string input, out T1 argument);
	bool TryParse<T1, T2>(Delegate @delegate, string input, out ValueTuple<T1, T2> argument);
	bool TryParse<T1, T2, T3>(Delegate @delegate, string input, out ValueTuple<T1, T2, T3> argument);
	bool TryParse<T1, T2, T3, T4>(Delegate @delegate, string input, out ValueTuple<T1, T2, T3, T4> argument);
	bool TryParse<T1, T2, T3, T4, T5>(Delegate @delegate, string input, out ValueTuple<T1, T2, T3, T4, T5> argument);
	bool TryParse<T1, T2, T3, T4, T5, T6>(Delegate @delegate, string input, out ValueTuple<T1, T2, T3, T4, T5, T6> argument);
	bool TryParse<T1, T2, T3, T4, T5, T6, T7>(Delegate @delegate, string input, out ValueTuple<T1, T2, T3, T4, T5, T6, T7> argument);
}

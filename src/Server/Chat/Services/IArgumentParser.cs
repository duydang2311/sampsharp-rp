namespace Server.Chat.Services;

public interface IArgumentParser
{
	bool TryParse(Delegate @delegate, string input, out object?[]? arguments);
	bool TryParse<T1>(string input, out T1 argument);
	bool TryParse<T1, T2>(string input, out ValueTuple<T1, T2> argument);
	bool TryParse<T1, T2, T3>(string input, out ValueTuple<T1, T2, T3> argument);
	bool TryParse<T1, T2, T3, T4>(string input, out ValueTuple<T1, T2, T3, T4> argument);
	bool TryParse<T1, T2, T3, T4, T5>(string input, out ValueTuple<T1, T2, T3, T4, T5> argument);
	bool TryParse<T1, T2, T3, T4, T5, T6>(string input, out ValueTuple<T1, T2, T3, T4, T5, T6> argument);
	bool TryParse<T1, T2, T3, T4, T5, T6, T7>(string input, out ValueTuple<T1, T2, T3, T4, T5, T6, T7> argument);
}

using System.ComponentModel;
using System.Globalization;

namespace Server.Chat.Services;

public sealed class CommandArgumentParser : IArgumentParser
{
	private readonly int prefixCount;

	public CommandArgumentParser() { }

	public CommandArgumentParser(int prefixCount)
	{
		this.prefixCount = prefixCount;
	}

	private static bool TryConvertInternal(string value, Type type, out object? result)
	{
		var underlying = Nullable.GetUnderlyingType(type);
		if (underlying is not null)
		{
			if (TypeDescriptor.GetConverter(underlying).IsValid(value))
			{
				result = Convert.ChangeType(value, underlying, CultureInfo.InvariantCulture);
				return true;
			}
			result = default;
			return string.IsNullOrEmpty(value);
		}
		if (TypeDescriptor.GetConverter(type).IsValid(value))
		{
			result = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
			return true;
		}
		result = default;
		return false;
	}

	private static bool TryParseTypesInternal(Type[] types, string input, out object?[]? arguments)
	{
		var typesCount = types.Length;
		if (typesCount == 0)
		{
			arguments = default;
			return true;
		}
		var splitted = string.IsNullOrEmpty(input)
			? Array.Empty<string>()
			: input.Split(' ', typesCount);
		var splittedCount = splitted.Length;
		var results = new object?[typesCount];
		if (typesCount > splittedCount)
		{
			var i = 0;
			foreach (var type in types[splittedCount..])
			{
				if (Nullable.GetUnderlyingType(type) is not null)
				{
					results[splittedCount + i++] = null;
					continue;
				}
				arguments = default;
				return false;
			}
		}
		for (var i = 0; i != splittedCount; ++i)
		{
			if (!TryConvertInternal(splitted[i], types[i], out results[i]))
			{
				arguments = default;
				return false;
			}
		}
		arguments = results;
		return true;
	}

	public bool TryParse(Delegate @delegate, string input, out object?[]? arguments)
	{
		var method = @delegate.Method;
		var parameters = method.GetParameters()[prefixCount..];
		var parameterCount = parameters.Length;
		if (parameterCount == 0)
		{
			arguments = default;
			return true;
		}
		var splitted = string.IsNullOrEmpty(input)
			? Array.Empty<string>()
			: input.Split(' ', parameterCount);
		var splittedCount = splitted.Length;
		var results = new object?[parameterCount];
		if (parameterCount > splittedCount)
		{
			var i = 0;
			foreach (var parameter in parameters[splittedCount..])
			{
				if (parameter.HasDefaultValue)
				{
					results[splittedCount + i++] = parameter.DefaultValue;
					continue;
				}
				if (Nullable.GetUnderlyingType(parameter.ParameterType) is not null)
				{
					results[splittedCount + i++] = null;
					continue;
				}
				arguments = default;
				return false;
			}
		}
		for (var i = 0; i != splittedCount; ++i)
		{
			if (!TryConvertInternal(splitted[i], parameters[i].ParameterType, out results[i]))
			{
				arguments = default;
				return false;
			}
		}
		arguments = results;
		return true;
	}

	public bool TryParse<T1>(string input, out T1 argument)
	{
		var success = TryParseTypesInternal(new[] { typeof(T1) }, input, out var arguments);
		argument = !success || arguments is null
			? default!
			: (T1)arguments![0]!;
		return success;
	}

	public bool TryParse<T1, T2>(string input, out ValueTuple<T1, T2> argument)
	{
		var success = TryParseTypesInternal(new[] { typeof(T1), typeof(T2) }, input, out var arguments);
		argument = !success || arguments is null
			? default!
			: ((T1)arguments![0]!, (T2)arguments[1]!);
		return success;
	}

	public bool TryParse<T1, T2, T3>(string input, out ValueTuple<T1, T2, T3> argument)
	{
		var success = TryParseTypesInternal(new[] { typeof(T1), typeof(T2), typeof(T3) }, input, out var arguments);
		argument = !success || arguments is null
			? default!
			: ((T1)arguments![0]!, (T2)arguments[1]!, (T3)arguments[2]!);
		return success;
	}

	public bool TryParse<T1, T2, T3, T4>(string input, out ValueTuple<T1, T2, T3, T4> argument)
	{
		var success = TryParseTypesInternal(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, input, out var arguments);
		argument = !success || arguments is null
			? default!
			: ((T1)arguments![0]!, (T2)arguments[1]!, (T3)arguments[2]!, (T4)arguments[3]!);
		return success;
	}

	public bool TryParse<T1, T2, T3, T4, T5>(string input, out ValueTuple<T1, T2, T3, T4, T5> argument)
	{
		var success = TryParseTypesInternal(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }, input, out var arguments);
		argument = !success || arguments is null
			? default!
			: ((T1)arguments![0]!, (T2)arguments[1]!, (T3)arguments[2]!, (T4)arguments[3]!, (T5)arguments[4]!);
		return success;
	}

	public bool TryParse<T1, T2, T3, T4, T5, T6>(string input, out ValueTuple<T1, T2, T3, T4, T5, T6> argument)
	{
		var success = TryParseTypesInternal(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) }, input, out var arguments);
		argument = !success || arguments is null
			? default!
			: ((T1)arguments![0]!, (T2)arguments[1]!, (T3)arguments[2]!, (T4)arguments[3]!, (T5)arguments[4]!, (T6)arguments[5]!);
		return success;
	}

	public bool TryParse<T1, T2, T3, T4, T5, T6, T7>(string input, out ValueTuple<T1, T2, T3, T4, T5, T6, T7> argument)
	{
		var success = TryParseTypesInternal(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) }, input, out var arguments);
		argument = !success || arguments is null
			? default!
			: ((T1)arguments![0]!, (T2)arguments[1]!, (T3)arguments[2]!, (T4)arguments[3]!, (T5)arguments[4]!, (T6)arguments[5]!, (T7)arguments[6]!);
		return success;
	}
}

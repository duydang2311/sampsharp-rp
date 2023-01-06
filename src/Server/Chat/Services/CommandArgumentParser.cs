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

	public bool TryParse(Delegate @delegate, string input, out object?[]? arguments)
	{
		var method = @delegate.Method;
		var parameters = method.GetParameters().Skip(prefixCount).ToArray();
		var parameterCount = parameters.Length;
		if (parameterCount == 0)
		{
			arguments = default;
			return true;
		}
		var splitted = input.Split(' ', parameterCount);
		var splittedCount = splitted.Length;
		var results = new object?[parameterCount];
		if (parameterCount > splittedCount)
		{
			var i = 0;
			foreach (var parameter in parameters.Skip(splittedCount))
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

	public bool TryParse<T1>(Delegate @delegate, string input, out T1 argument)
	{
		var success = TryParse(@delegate, input, out object?[]? arguments);
		if (!success || arguments is null)
		{
			argument = default!;
		}
		else
		{
			argument = (T1)arguments![0]!;
		}
		return success;
	}

	public bool TryParse<T1, T2>(Delegate @delegate, string input, out (T1, T2) argument)
	{
		var success = TryParse(@delegate, input, out object?[]? arguments);
		if (!success || arguments is null)
		{
			argument = default!;
		}
		else
		{
			argument = ((T1)arguments[0]!, (T2)arguments[1]!);
		}
		return success;
	}

	public bool TryParse<T1, T2, T3>(Delegate @delegate, string input, out (T1, T2, T3) argument)
	{
		var success = TryParse(@delegate, input, out object?[]? arguments);
		if (!success || arguments is null)
		{
			argument = default!;
		}
		else
		{
			argument = ((T1)arguments[0]!, (T2)arguments[1]!, (T3)arguments[2]!);
		}
		return success;
	}

	public bool TryParse<T1, T2, T3, T4>(Delegate @delegate, string input, out (T1, T2, T3, T4) argument)
	{
		var success = TryParse(@delegate, input, out object?[]? arguments);
		if (!success || arguments is null)
		{
			argument = default!;
		}
		else
		{
			argument = ((T1)arguments[0]!, (T2)arguments[1]!, (T3)arguments[2]!, (T4)arguments[3]!);
		}
		return success;
	}

	public bool TryParse<T1, T2, T3, T4, T5>(Delegate @delegate, string input, out (T1, T2, T3, T4, T5) argument)
	{
		var success = TryParse(@delegate, input, out object?[]? arguments);
		if (!success || arguments is null)
		{
			argument = default!;
		}
		else
		{
			argument = ((T1)arguments[0]!, (T2)arguments[1]!, (T3)arguments[2]!, (T4)arguments[3]!, (T5)arguments[4]!);
		}
		return success;
	}

	public bool TryParse<T1, T2, T3, T4, T5, T6>(Delegate @delegate, string input, out (T1, T2, T3, T4, T5, T6) argument)
	{
		var success = TryParse(@delegate, input, out object?[]? arguments);
		if (!success || arguments is null)
		{
			argument = default!;
		}
		else
		{
			argument = ((T1)arguments[0]!, (T2)arguments[1]!, (T3)arguments[2]!, (T4)arguments[3]!, (T5)arguments[4]!, (T6)arguments[5]!);
		}
		return success;
	}

	public bool TryParse<T1, T2, T3, T4, T5, T6, T7>(Delegate @delegate, string input, out (T1, T2, T3, T4, T5, T6, T7) argument)
	{
		var success = TryParse(@delegate, input, out object?[]? arguments);
		if (!success || arguments is null)
		{
			argument = default!;
		}
		else
		{
			argument = ((T1)arguments[0]!, (T2)arguments[1]!, (T3)arguments[2]!, (T4)arguments[3]!, (T5)arguments[4]!, (T6)arguments[5]!, (T7)arguments[6]!);
		}
		return success;
	}
}

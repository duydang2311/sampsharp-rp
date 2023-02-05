namespace System
{
	public static class ArrayDeconstructionExtensions
	{
		private static T? Get<T>(T[] array, int index)
		{
			return index >= array.Length ? default : array[index];
		}
		public static void Deconstruct<T>(this T[] array, out T? first, out T[] rest)
		{
			first = Get(array, 0);
			rest = GetRestOfArray(array, 1);
		}
		public static void Deconstruct<T>(this T[] array, out T? first, out T? second, out T[] rest)
		{
			first = Get(array, 0);
			second = Get(array, 1);
			rest = GetRestOfArray(array, 2);
		}
		public static void Deconstruct<T>(this T[] array, out T? first, out T? second, out T? third, out T[] rest)
		{
			first = Get(array, 0);
			second = Get(array, 1);
			third = Get(array, 2);
			rest = GetRestOfArray(array, 3);
		}
		public static void Deconstruct<T>(this T[] array, out T? first, out T? second, out T? third, out T? fourth, out T[] rest)
		{
			first = Get(array, 0);
			second = Get(array, 1);
			third = Get(array, 2);
			fourth = Get(array, 3);
			rest = GetRestOfArray(array, 4);
		}
		public static void Deconstruct<T>(this T[] array, out T? first, out T? second, out T? third, out T? fourth, out T? fifth, out T[] rest)
		{
			first = Get(array, 0);
			second = Get(array, 1);
			third = Get(array, 2);
			fourth = Get(array, 3);
			fifth = Get(array, 4);
			rest = GetRestOfArray(array, 5);
		}
		public static void Deconstruct<T>(this T[] array, out T? first, out T? second, out T? third, out T? fourth, out T? fifth, out T? sixth, out T[] rest)
		{
			first = Get(array, 0);
			second = Get(array, 1);
			third = Get(array, 2);
			fourth = Get(array, 3);
			fifth = Get(array, 4);
			sixth = Get(array, 5);
			rest = GetRestOfArray(array, 6);
		}
		public static void Deconstruct<T>(this T[] array, out T? first, out T? second, out T? third, out T? fourth, out T? fifth, out T? sixth, out T? seventh, out T[] rest)
		{
			first = Get(array, 0);
			second = Get(array, 1);
			third = Get(array, 2);
			fourth = Get(array, 3);
			fifth = Get(array, 4);
			sixth = Get(array, 5);
			seventh = Get(array, 6);
			rest = GetRestOfArray(array, 7);
		}
		public static void Deconstruct<T>(this T[] array, out T? first, out T? second, out T? third, out T? fourth, out T? fifth, out T? sixth, out T? seventh, out T? eighth, out T[] rest)
		{
			first = Get(array, 0);
			second = Get(array, 1);
			third = Get(array, 2);
			fourth = Get(array, 3);
			fifth = Get(array, 4);
			sixth = Get(array, 5);
			seventh = Get(array, 6);
			eighth = Get(array, 7);
			rest = GetRestOfArray(array, 8);
		}
		private static T[] GetRestOfArray<T>(T[] array, int skip)
		{
			return skip >= array.Length ? Array.Empty<T>() : array.Skip(skip).ToArray();
		}
	}
}

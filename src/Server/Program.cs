using SampSharp.Core;
using SampSharp.Entities;

namespace Server
{
	public class Program
	{
		private static void Main()
		{
			new GameModeBuilder()
				.UseEcs<Startup>()
				.Run();
		}
	}
}

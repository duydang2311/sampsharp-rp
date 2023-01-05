using System.Text;
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
				.UseEncoding(Encoding.UTF8)
				.Run();
		}
	}
}

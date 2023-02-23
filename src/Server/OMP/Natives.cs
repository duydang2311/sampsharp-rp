using System.Runtime.InteropServices;
using System.Text;

namespace Server.OMP;

public static class Natives
{
	[DllImport("libempty-template.so", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	private extern static bool GetPlayerName(int playerid, [Out] StringBuilder name, int size);
	[DllImport("libempty-template.so", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	private extern static bool GetPlayerName(int playerid, [Out] IntPtr ptr, int size);

	[DllImport("libempty-template.so", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	private extern static IntPtr GetPlayerName2(int playerid);
	public static string GetPlayerNameImpl1(int playerid)
	{
		var builder = new StringBuilder(32);
		GetPlayerName(playerid, builder, 32);
		return builder.ToString();
	}
	public static string GetPlayerNameImpl2(int playerid)
	{
		var ptr = Marshal.AllocHGlobal(32);
		GetPlayerName(playerid, ptr, 32);
		var name = Marshal.PtrToStringAnsi(ptr) ?? string.Empty;
		Marshal.FreeHGlobal(ptr);
		ptr = IntPtr.Zero;
		return name;
	}
	public static string GetPlayerNameImpl3(int playerid)
	{
		return Marshal.PtrToStringAnsi(GetPlayerName2(playerid)) ?? string.Empty;
	}

	[DllImport("libempty-template.so", CallingConvention = CallingConvention.Cdecl)]
	public extern static bool SpawnPlayer(int playerid);

	[DllImport("libempty-template.so", CallingConvention = CallingConvention.Cdecl)]
	public extern static bool Print(string text);

	[DllImport("libempty-template.so", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	public extern static IntPtr Test();

	[DllImport("libempty-template.so", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
	public extern static void TestConst([Out][MarshalAs(UnmanagedType.LPWStr)] out string name);

	[DllImport("libempty-template.so", CallingConvention = CallingConvention.Cdecl)]
	public extern static bool SetPlayerPosition(int playerid, float x, float y, float z);
}

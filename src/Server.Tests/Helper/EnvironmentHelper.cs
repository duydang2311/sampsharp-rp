namespace Server.Tests.Helper;

public static class EnvironmentHelper
{
	private static readonly bool isOnCI;
	public static bool IsOnCI => isOnCI;
	static EnvironmentHelper()
	{
		isOnCI = Environment.GetEnvironmentVariable("CI") is not null;
	}
}

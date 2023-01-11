using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Unidecode.NET;

namespace Server.ResourceGenerator.Text;

public static class VietnameseUnidecodedGenerator
{
	public static void Generate()
	{
		var serverAssembly = Assembly.Load("Server");
		var accentedResource = serverAssembly.GetSatelliteAssembly(CultureInfo.GetCultureInfo("vi-VN"));
		var stream = accentedResource.GetManifestResourceStream("Server.Resources.Text.vi-VN.resources");
		if (stream is null)
		{
			return;
		}
		var reader = new ResourceReader(stream);
		using var writer = new ResourceWriter("../Server/Resources/Text.vi.resources");
		foreach (DictionaryEntry i in reader)
		{
			writer.AddResource(i.Key.ToString()!, i.Value!.ToString()!.Unidecode());
		}
		writer.Close();
	}
}

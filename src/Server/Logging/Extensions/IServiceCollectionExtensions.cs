using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class IServiceCollectionExtensions
{
	public static IServiceCollection WithLogging(this IServiceCollection self)
	{
		return self.AddLogging(logging =>
		{
			var template = "{@l:w4}:" +
				"{#if SourceContext is not null} {Substring(SourceContext, IndexOf(SourceContext, '.') + 1)}[{@i}]{#end} ({@t:HH:mm:ss})\n" +
				"{#if Length(Scope) > 0}" +
				"      {#each s in Scope}=> {s}{#delimit} {#end}\n" +
				"{#end}" +
				"      {@m}\n" +
				"{@x}";
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.Enrich.FromLogContext()
				.WriteTo.Console(new ExpressionTemplate(template, theme: TemplateTheme.Code))
				.WriteTo.File(new ExpressionTemplate(template), "logs/server-.txt", rollingInterval: RollingInterval.Day)
				.CreateLogger();

			logging.ClearProviders();
			logging.AddSerilog(dispose: true);
			logging.SetMinimumLevel(LogLevel.Information);
		});
	}
}

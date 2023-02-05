using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace Server.I18N.Localization.Services;

public class TextNameIdentifierService : ITextNameIdentifierService
{
	private readonly ILogger<TextNameIdentifierService> logger;

	public TextNameIdentifierService(ILogger<TextNameIdentifierService> logger)
	{
		this.logger = logger;
	}

	public string Identify<T>(Expression<Func<T, object>> identifier)
	{
		switch (identifier.Body)
		{
			case MemberExpression expression:
				{
					return expression.Member.Name;
				}
			case NewExpression expression:
				{
					logger.LogWarning("Using NewExpression in Identify() is not supported; Fallback to only identify the first text name in collection if there's any");
					if (expression.Members is null)
					{
						return string.Empty;
					}
					return expression.Members.FirstOrDefault()?.Name ?? string.Empty;
				}
		}
		logger.LogWarning("Identify() is used with invalid expression. Fallback to return empty string");
		return string.Empty;
	}
}

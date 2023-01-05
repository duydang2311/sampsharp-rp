using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using SampSharp.Entities.SAMP.Commands;
using Server.Chat.Services;

namespace Server.Chat.Middlewares;

public sealed class PlayerCommandMiddleware
{
	private readonly EventDelegate _next;

	public PlayerCommandMiddleware(EventDelegate next)
	{
		_next = next;
	}

	public object Invoke(EventContext context, ICustomPlayerCommandService commandService, ILogger<PlayerCommandMiddleware> logger)
	{
		var response = _next(context);
		if (EventHelper.IsSuccessResponse(response))
			return response;

		if (context.Arguments[0] is not EntityId entity || context.Arguments[1] is not string text)
		{
			var exception = new ArgumentException("Invalid command middleware input argument types!");
			logger.LogError(exception, "Invalid command input arguments");
			throw exception;
		}
		var result = commandService.Invoke(context.EventServices, entity, text);
		if (result.Response == InvokeResponse.Success)
		{
			return true;
		}

		var player = context.EventServices.GetRequiredService<IEntityManager>().GetComponent<Player>(entity);
		if (result.Response == InvokeResponse.InvalidArguments)
		{
			player.SendClientMessage(Color.Gold, result.UsageMessage);
		}
		else
		{
			player.SendClientMessage(Color.Red, "Lỗi: Lệnh không tồn tại.");
		}
		return true;
	}
}

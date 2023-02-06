using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;

namespace Server.Chat.Middlewares;

public sealed partial class TextMiddleware
{
	private readonly EventDelegate next;

	public TextMiddleware(EventDelegate next)
	{
		this.next = next;
	}

	[LoggerMessage(
		EventId = 0,
		Level = LogLevel.Error,
		Message = "Invalid command middleware input argument types!")]
	public static partial void LogInvalidArguments(ILogger<CommandMiddleware> logger);

	public object Invoke(EventContext context, ILogger<CommandMiddleware> logger, IChatService chatService,
		IEntityManager entityManager)
	{
		var response = next(context);
		if (EventHelper.IsSuccessResponse(response))
		{
			return response;
		}

		if (context.Arguments[0] is not EntityId entity || context.Arguments[1] is not string input)
		{
			LogInvalidArguments(logger);
			return 0;
		}

		var sourcePlayer = entityManager.GetComponent<Player>(entity);
		var distanceSquared = 15f * 15f;
		chatService.SendMessage(
			player => Vector3.DistanceSquared(player.Position, sourcePlayer.Position) <= distanceSquared,
			b => b.Add(i => i.Chat_Message, sourcePlayer.Name, input)
		);
		return 0;
	}
}

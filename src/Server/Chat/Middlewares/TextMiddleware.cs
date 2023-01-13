using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;

namespace Server.Chat.Middlewares;

public sealed class TextMiddleware
{
    private readonly EventDelegate next;

    public TextMiddleware(EventDelegate next)
    {
        this.next = next;
    }

    public object Invoke(EventContext context, ILogger<CommandMiddleware> logger, IChatService chatService, IEntityManager entityManager)
    {
        var response = next(context);
        if (EventHelper.IsSuccessResponse(response))
            return response;
        if (context.Arguments[0] is not EntityId entity || context.Arguments[1] is not string input)
        {
            var exception = new ArgumentException("Invalid command middleware input argument types!");
            logger.LogError(exception, "");
            throw exception;
        }

        var sourcePlayer = entityManager.GetComponent<Player>(entity);
        var distanceSquared = 15f * 15f;
        chatService.SendMessage(player => Vector3.DistanceSquared(player.Position, sourcePlayer.Position) <= distanceSquared, f => f.Create(i => i.ChatMessage, sourcePlayer.Name, input));
        return 0;
    }
}
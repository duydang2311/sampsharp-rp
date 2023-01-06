using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;

namespace Server.Chat.Middlewares;

public sealed class CommandMiddleware
{
	private readonly EventDelegate _next;

	public CommandMiddleware(EventDelegate next)
	{
		_next = next;
	}

	private static void InvokeHelper(string command, Player player, ICommandService commandService, ILogger<CommandMiddleware> logger)
	{
		if (commandService.HasHelper(command))
		{
			commandService.InvokeHelper(command, new object[] { player });
		}
		else
		{
			logger.LogWarning("Command {commandName} is incorrectly used but missing helper method", command);
		}
	}

	public object Invoke(EventContext context, ICommandService commandService, IArgumentParser parser, ILogger<CommandMiddleware> logger)
	{
		var response = _next(context);
		if (EventHelper.IsSuccessResponse(response))
			return response;

		if (context.Arguments[0] is not EntityId entity || context.Arguments[1] is not string input)
		{
			var exception = new ArgumentException("Invalid command middleware input argument types!");
			logger.LogError(exception, "");
			throw exception;
		}

		var player = context.EventServices.GetRequiredService<IEntityManager>().GetComponent<Player>(entity);
		if (!commandService.HasCommand(context.Name))
		{
			player.SendClientMessage(Color.Red, "[Hệ thống] Lệnh không tồn tại.");
			return true;
		}
		if (!parser.TryParse(commandService.GetCommandDelegate(context.Name), input, out var arguments))
		{
			InvokeHelper(context.Name, player, commandService, logger);
			return true;
		}

		arguments = arguments is null
			? new object[] { player }
			: arguments.Prepend(player).ToArray();
		var result = commandService.InvokeCommand(context.Name, arguments);
		switch (result)
		{
			case Task task:
				{
					task.ContinueWith(t =>
					{
						var exception = t.Exception!;
						logger.LogError(exception, "");
						throw exception;
					}, TaskContinuationOptions.OnlyOnFaulted);
					break;
				}
			case bool ok:
				{
					if (!ok)
					{
						InvokeHelper(context.Name, player, commandService, logger);
					}
					break;
				}
		}
		return true;
	}
}

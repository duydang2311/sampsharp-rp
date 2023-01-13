using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Components;
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

	public object Invoke(EventContext context, ICommandService commandService, IArgumentParser parser, ILogger<CommandMiddleware> logger, IChatService chatService)
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

		input = input.Trim();
		var whitespaceIndex = input.IndexOf(' ');
		var player = context.EventServices.GetRequiredService<IEntityManager>().GetComponent<Player>(entity);
		string command;
		if (whitespaceIndex == -1)
		{
			command = input[1..];
			input = string.Empty;
		}
		else
		{
			command = input[1..whitespaceIndex];
			input = input[(whitespaceIndex + 1)..];
		}
		if (!commandService.HasCommand(command))
		{
			chatService.SendInlineMessages(player, f => new[] {
				f.Create(Color.Gray, m => m.BadgeSystem),
				f.Create(Color.Red, m => m.CommandNotFound),
			});
			return true;
		}

		var model = commandService.GetCommandModel(command);
		var @delegate = model.Delegate;
		if (@delegate is null)
		{
			return true;
		}
		var permissionComponent = player.GetComponent<PermissionComponent>();
		if (permissionComponent is null
		|| (permissionComponent.Level & model.PermissionLevel) == 0)
		{
			chatService.SendInlineMessages(player, f => new[] {
				f.Create(Color.Gray, m => m.BadgeSystem),
				f.Create(Color.Red, m => m.CommandDenied),
			});
			return true;
		}
		if (!parser.TryParse(@delegate, input, out var arguments))
		{
			InvokeHelper(command, player, commandService, logger);
			return true;
		}
		arguments = arguments is null
			? new object[] { player }
			: arguments.Prepend(player).ToArray();
		var result = commandService.InvokeCommand(command, arguments);
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
						InvokeHelper(command, player, commandService, logger);
					}
					break;
				}
		}
		return true;
	}
}

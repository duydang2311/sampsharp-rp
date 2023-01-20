using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Components;
using Server.Chat.Services;
using Server.Common.Colors;
using Server.Database;
using Server.Door.Services;

namespace Server.Door.Systems.DoorCommand;

public sealed partial class DoorCommandSystem : ISystem
{
	private readonly IArgumentParser argumentParser;
	private readonly IChatService chatService;
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly DoorFactory doorFactory;

	public DoorCommandSystem(ICommandService commandService, IChatService chatService, IArgumentParser argumentParser, IDbContextFactory<ServerDbContext> dbContextFactory, DoorFactory doorFactory)
	{
		this.chatService = chatService;
		this.argumentParser = argumentParser;
		this.dbContextFactory = dbContextFactory;
		this.doorFactory = doorFactory;

		commandService.RegisterCommand(m =>
		{
			m.Name = "door";
			m.Delegate = DoorCommand;
			m.PermissionLevel = PermissionLevel.Admin;
		});
		commandService.RegisterHelper("door", HelpDoorCommand);
	}

	private void HelpDoorCommand(Player player)
	{
		chatService.SendMessage(player, SemanticColor.Info, m => m.DoorCommand_Help);
		chatService.SendMessage(player, Color.Gray, m => m.DoorCommand_Options);
	}

	private Task DoorCommand(Player player, string argument)
	{
		var (option, rest, _) = argument.Split(' ', 2);
		switch (option)
		{
			case "create":
				{
					return CreateDoor(player, rest);
				}
			case "destroy":
				{
					break;
				}
			case "nearby":
				{
					break;
				}
			default:
				{
					HelpDoorCommand(player);
					break;
				}
		}
		return Task.CompletedTask;
	}
}

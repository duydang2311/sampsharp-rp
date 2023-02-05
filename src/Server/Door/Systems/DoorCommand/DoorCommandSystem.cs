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
	private readonly IChatService chatService;
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly IDoorFactory doorFactory;

	public DoorCommandSystem(ICommandService commandService, IChatService chatService, IDbContextFactory<ServerDbContext> dbContextFactory, IDoorFactory doorFactory)
	{
		this.chatService = chatService;
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
		chatService.SendMessage(player, b => b
			.AddBadge(t => t.Badge_CommandUsage)
			.Inline(t => t.DoorCommand_Help)
			.Add(Color.Gray, t => t.DoorCommand_Options));
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
					return DestroyDoor(player, rest);
				}
			case "nearby":
				{
					return NearbyDoor(player, rest);
				}
			case "entrance":
				{
					UpdateDoorEntrance(player, rest);
					break;
				}
			case "exit":
				{
					UpdateDoorExit(player, rest);
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

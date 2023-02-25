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
			m.PermissionLevel = PermissionLevel.Admin;
			m.Delegate = DoorCommand;
			m.HelpDelegate = HelpDoorCommand;
		});
	}

	private void HelpDoorCommand(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_Help)
			.Inline(t => t.DoorCommand_Help)
			.Add(SemanticColor.LowAttention, t => t.DoorCommand_Options));
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
					return UpdateDoorEntrance(player, rest);
				}
			case "exit":
				{
					return UpdateDoorExit(player, rest);
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

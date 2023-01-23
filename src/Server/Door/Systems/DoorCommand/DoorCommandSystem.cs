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
	private readonly IChatMessageBuilderFactory chatMessageBuilderFactory;

	public DoorCommandSystem(ICommandService commandService, IChatService chatService, IDbContextFactory<ServerDbContext> dbContextFactory, IDoorFactory doorFactory, IChatMessageBuilderFactory chatMessageBuilderFactory)
	{
		this.chatService = chatService;
		this.dbContextFactory = dbContextFactory;
		this.doorFactory = doorFactory;
		this.chatMessageBuilderFactory = chatMessageBuilderFactory;

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
			.Add(SemanticColor.Info, m => m.DoorCommand_Help)
			.Add(Color.Gray, m => m.DoorCommand_Options));
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
			default:
				{
					HelpDoorCommand(player);
					break;
				}
		}
		return Task.CompletedTask;
	}
}

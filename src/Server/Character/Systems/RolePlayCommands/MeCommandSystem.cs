using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Components;
using Server.Chat.Models;
using Server.Chat.Services;

namespace Server.Character.Systems.RolePlayCommands;

public sealed class MeCommandSystem : ISystem
{
	private readonly IChatService chatService;

	public MeCommandSystem(ICommandService commandService, IChatService chatService)
	{
		this.chatService = chatService;
		commandService.RegisterCommand((model) =>
		{
			model.Name = "me";
			model.Delegate = MeCommand;
		});
		commandService.RegisterHelper("me", HelpMeCommand);
	}

	public void MeCommand(Player player, string text)
	{
		string message = $"* {player.Name} {text}";
//        player.SendClientMessage(Color.FromInteger(0xC2A2DA, ColorFormat.RGB), message);
		chatService.SendMessage(
			p => (Vector3.DistanceSquared(p.Position, player.Position) < 15f * 15f),
			factory => factory.Create(Color.FromInteger(0xC2A2DA, ColorFormat.RGB), message));
	}

// 123 gh
	public void HelpMeCommand(Player player)
	{
		chatService.SendMessage(
			player, factory => factory.Create(Color.FromInteger(0xC2A2DA, ColorFormat.RGB), "Usage: /me [action]."));
	}
}
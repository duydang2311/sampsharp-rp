using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;
using Server.Common.Colors;

namespace Server.Character.Systems.RolePlayCommands;

public sealed class MeCommandSystem : ISystem
{
	private readonly IChatService chatService;

	public MeCommandSystem(ICommandService commandService, IChatService chatService)
	{
		this.chatService = chatService;
		commandService.RegisterCommand(m =>
		{
			m.Name = "me";
			m.Delegate = MeCommand;
			m.HelpDelegate = HelpMeCommand;
		});
	}

	public void MeCommand(Player player, string text)
	{
		chatService.SendMessage(
			p => Vector3.DistanceSquared(p.Position, player.Position) < 15f * 15f,
			b => b.Add(Color.FromInteger(0xC2A2DA, ColorFormat.RGB), model => model.MeCommandText, player.Name, text));
	}


	public void HelpMeCommand(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(model => model.Badge_Help)
			.Inline(model => model.MeCommandHelp));
	}
}

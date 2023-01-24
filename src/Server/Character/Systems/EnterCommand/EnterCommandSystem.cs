using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;

namespace Server.Character.Systems.EnterCommand;

public sealed class EnterCommandSystem : ISystem
{
	private readonly IEnterCommandEvent enterCommandEvent;

	public EnterCommandSystem(ICommandService commandService, IEnterCommandEvent enterCommandEvent)
	{
		this.enterCommandEvent = enterCommandEvent;

		commandService.RegisterCommand(m =>
		{
			m.Name = "enter";
			m.Delegate = EnterCommand;
		});
	}

	private async void EnterCommand(Player player)
	{
		await enterCommandEvent.InvokeAsync(player);
	}
}

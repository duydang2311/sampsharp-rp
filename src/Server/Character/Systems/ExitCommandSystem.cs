using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;

namespace Server.Character.Systems.ExitCommand;

public sealed class ExitCommandSystem : ISystem
{
	private readonly IExitCommandEvent exitCommandEvent;

	public ExitCommandSystem(ICommandService commandService, IExitCommandEvent exitCommandEvent)
	{
		this.exitCommandEvent = exitCommandEvent;

		commandService.RegisterCommand(m =>
		{
			m.Name = "exit";
			m.Delegate = ExitCommand;
		});
	}

	private async void ExitCommand(Player player)
	{
		await exitCommandEvent.InvokeAsync(player);
	}
}

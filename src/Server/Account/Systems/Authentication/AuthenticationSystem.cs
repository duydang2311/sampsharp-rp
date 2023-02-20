using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;
using Server.Database;


namespace Server.Account.Systems.Authentication;

public sealed class AuthenticationSystem : IAuthenticationSystem
{
	private readonly IDbContextFactory<ServerDbContext> contextFactory;
	private readonly IChatService chatService;
	private readonly IAuthenticatedEvent authenticatedEvent;

	public AuthenticationSystem(IDbContextFactory<ServerDbContext> contextFactory, IChatService chatService, IAuthenticatedEvent authenticatedEvent)
	{
		this.contextFactory = contextFactory;
		this.chatService = chatService;
		this.authenticatedEvent = authenticatedEvent;
	}

	[Event]
	private async void OnPlayerConnect(Player player)
	{
		chatService.SendMessage(player, b => b
			.Add(t => t.Badge_System)
			.Inline(t => t.Account_Authentication_Loading));
		player.ToggleSpectating(true);
		await AuthenticateAsync(player).ConfigureAwait(false);
	}

	public async Task AuthenticateAsync(Player player)
	{
		await using var context = await contextFactory.CreateDbContextAsync().ConfigureAwait(false);
		var any = await context.Accounts.AnyAsync(model => player.Name == model.Name).ConfigureAwait(false);
		await authenticatedEvent.InvokeAsync(player, any).ConfigureAwait(false);
	}
}

using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Chat.Services;
using Server.Database;


namespace Server.Account.Systems.Authentication;

public sealed class AuthenticationSytem : ISystem
{
	private readonly IDbContextFactory<ServerDbContext> contextFactory;
	private readonly IChatService chatService;
	private readonly IAuthenticatedEvent authenticatedEvent;

	public AuthenticationSytem(IDbContextFactory<ServerDbContext> contextFactory, IChatService chatService,
		IAuthenticatedEvent authenticatedEvent)
	{
		this.contextFactory = contextFactory;
		this.chatService = chatService;
		this.authenticatedEvent = authenticatedEvent;
	}

    [Event]
    private async void OnPlayerConnect(Player player)
    {
        chatService.SendMessage(player, f => f.Add(m => m.Account_Authentication_Loading));
        player.ToggleSpectating(true);
        await using var context = await contextFactory.CreateDbContextAsync();
        var exist = await context.Accounts.AnyAsync(model => player.Name == model.Name);
        await authenticatedEvent.InvokeAsync(player, exist);
    }
}
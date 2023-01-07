using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Systems.Login;
using Server.Account.Systems.SignUp;
using Server.Chat.Services;
using Server.Database;


namespace Server.Account.Systems.Authentication;

public sealed class AuthenticationSytem : ISystem
{
    private readonly IDbContextFactory<ServerDbContext> contextFactory;
    private readonly ILoginSystem loginSystem;
    private readonly ISignUpSystem signUpSystem;
    private readonly IChatService chatService;
    public AuthenticationSytem(IDbContextFactory<ServerDbContext> contextFactory, ILoginSystem loginSystem, ISignUpSystem signUpSystem, IChatService chatService)
    {
        this.contextFactory = contextFactory;
        this.loginSystem = loginSystem;
        this.signUpSystem = signUpSystem;
        this.chatService = chatService;
    }

    [Event]
    private void OnPlayerConnect(Player player)
    {
        chatService.SendMessage(player, (factory) => factory.Create("Đang tải, vui lòng đợi trong giây lát..."));
    }

    [Event]
    private async Task OnPlayerRequestClass(Player player, int classid)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var exist = await context.Characters.AnyAsync(model => player.Name == model.Name);
        player.ToggleSpectating(true);
        if (exist)
        {
            loginSystem.Login(player);
        }
        else
        {
            await signUpSystem.SignUp(player);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Components;
using Server.Account.Systems.Authentication;
using Server.Database;

namespace Server.Account.Systems.Login;

using BCrypt.Net;

public sealed class LoginSystem : ISystem
{
    private readonly IDbContextFactory<ServerDbContext> contextFactory;
    private readonly IDialogService dialogService;
    private readonly ILoginEvent loginEvent;

    public LoginSystem(IDbContextFactory<ServerDbContext> contextFactory, IDialogService dialogService,
        ILoginEvent loginEvent, IAuthenticatedEvent authenticatedEvent)
    {
        this.dialogService = dialogService;
        this.contextFactory = contextFactory;
        this.loginEvent = loginEvent;

        authenticatedEvent.AddHandler(OnPlayerAuthenticated);
    }

    public void OnPlayerAuthenticated(Player player, bool signedUp)
    {
        if (!signedUp)
        {
            return;
        }
        var loginDialog = new InputDialog()
        {
            Content =
                $"Chao mung tro lai, {player.Name}!\nHay nhap mat khau cua ban de dang nhap tai khoan va tham gia tro choi.",
            Caption = "<INSERT_NAME_SERVER> - Xac minh tai khoan",
            Button1 = "Dang nhap",
            Button2 = "Thoat",
            IsPassword = true
        };
        dialogService.Show(player, loginDialog, response =>
        {
            _ = Task.Run(async () =>
            {
                if (response.Response == DialogResponse.LeftButton)
                {
                    await using var context = await contextFactory.CreateDbContextAsync();
                    var account = (await context
                        .Accounts
                        .Where(model => player.Name == model.Name)
                        .Select(model => new { model.Password, model.Id })
                        .FirstOrDefaultAsync())!;
                    var success = await Task.Run(() => BCrypt.EnhancedVerify(response.InputText, account.Password));

                    if (success)
                    {
                        player.AddComponent<AccountComponent>(account.Id);
                        await loginEvent.InvokeAsync(player);
                    }
                    else
                    {
                        player.Kick();
                        // TODO: login again
                    }

                    return;
                }

                player.Kick();
            }).ContinueWith((t) => throw t.Exception!, TaskContinuationOptions.OnlyOnFaulted);
        });
    }
}
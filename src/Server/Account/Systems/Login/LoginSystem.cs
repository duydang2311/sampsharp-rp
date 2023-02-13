using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Components;
using Server.Account.Systems.Authentication;
using Server.Database;
using Server.SAMP.Dialog.Services;

namespace Server.Account.Systems.Login;

using BCrypt.Net;

public sealed class LoginSystem : ISystem
{
    private readonly IDbContextFactory<ServerDbContext> contextFactory;
    private readonly ICustomDialogService dialogService;
    private readonly ILoginEvent loginEvent;

    public LoginSystem(IDbContextFactory<ServerDbContext> contextFactory, ICustomDialogService dialogService,
        ILoginEvent loginEvent, IAuthenticatedEvent authenticatedEvent)
    {
        this.dialogService = dialogService;
        this.contextFactory = contextFactory;
        this.loginEvent = loginEvent;

        authenticatedEvent.AddHandler(OnPlayerAuthenticated);
    }

    public Task OnPlayerAuthenticated(Player player, bool signedUp)
    {
        if (!signedUp)
        {
            return Task.CompletedTask;
        }

        return ShowLoginDialogAsync(player);
    }

    private async Task ShowLoginDialogAsync(Player player)
    {
        var response = await dialogService.ShowAsync(player, f => f.CreateInput(dialog =>
        {
            dialog.Content =
                $"Chao mung tro lai, {player.Name}!\nHay nhap mat khau cua ban de dang nhap tai khoan va tham gia tro choi.";
            dialog.Caption = "<INSERT_NAME_SERVER> - Xac minh tai khoan";
            dialog.Button1 = "Dang nhap";
            dialog.Button2 = "Thoat";
            dialog.IsPassword = true;
        }));
        await OnLoginDialogResponse(player, response);
    }

    private async Task OnLoginDialogResponse(Player player, InputDialogResponse response)
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
                await ShowLoginDialogAsync(player);
            }

            return;
        }

        player.Kick();
    }
}
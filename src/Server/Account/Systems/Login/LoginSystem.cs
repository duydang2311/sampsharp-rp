using Microsoft.EntityFrameworkCore;
using SampSharp.Entities.SAMP;
using Server.Database;

namespace Server.Account.Systems.Login;
using BCrypt.Net;

public sealed class LoginSystem : ILoginSystem
{
    private readonly IDbContextFactory<ServerDbContext> contextFactory;
    private readonly IDialogService dialogService;
    private readonly ILoginEvent loginEvent;
    public LoginSystem(IDbContextFactory<ServerDbContext> contextFactory, IDialogService dialogService, ILoginEvent loginEvent)
    {
        this.dialogService = dialogService;
        this.contextFactory = contextFactory;
        this.loginEvent = loginEvent;
    }

    public void Login(Player player)
    {
        var loginDialog = new InputDialog()
        {
            Content = $"Chao mung tro lai, {player.Name}!\nHay nhap mat khau cua ban de dang nhap tai khoan va tham gia tro choi.",
            Caption = "<INSERT_NAME_SERVER> - Xac minh tai khoan",
            Button1 = "Dang nhap",
            Button2 = "Thoat",
            IsPassword = true
        };
        dialogService.Show(player, loginDialog, async response =>
        {
            if (response.Response == DialogResponse.LeftButton)
            {
                await using var context = await contextFactory.CreateDbContextAsync();
                var account = (await context.Accounts.Where(model => player.Name == model.Name).Select(model => new { model.Password, model.Id }).FirstOrDefaultAsync())!;
                var success = await Task.Run(() =>
                {
                    return BCrypt.EnhancedVerify(response.InputText, account.Password);
                });

                if (success)
                {
                    loginEvent.Invoke(player, account.Id);
                }
                else
                {
                    player.Kick();
                }
                return;
            }

            player.Kick();
        });
    }
}
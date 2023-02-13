using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Components;
using Server.Account.Systems.Authentication;
using Server.SAMP.Dialog.Services;

namespace Server.Account.Systems.SignUp;

using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Models;
using Database;

public sealed class SignUpSystem : ISystem
{
    private readonly IDbContextFactory<ServerDbContext> contextFactory;
    private readonly ICustomDialogService dialogService;
    private readonly ISignedUpEvent signedUpEvent;

    public SignUpSystem(IDbContextFactory<ServerDbContext> contextFactory, ICustomDialogService dialogService,
        IAuthenticatedEvent authenticatedEvent, ISignedUpEvent signedUpEvent)
    {
        this.contextFactory = contextFactory;
        this.dialogService = dialogService;
        this.signedUpEvent = signedUpEvent;

        authenticatedEvent.AddHandler(OnPlayerAuthenticated);
    }

    public Task OnPlayerAuthenticated(Player player, bool signedUp)
    {
        if (signedUp)
        {
            return Task.CompletedTask;
        }

        return ShowSignUpDialogAsync(player);
    }

    private async Task ShowSignUpDialogAsync(Player player)
    {
        var response = await dialogService.ShowAsync(player, f => f.CreateInput(dialog =>
        {
            dialog.Caption = "<INSERT_SERVER_NAME> - Dang ky tai khoan";
            dialog.Content = "{FFFFFF}Xin chao{7f9eba} " + player.Name +
                             "{FFFFFF}, tai khoan nay chua duoc dang ky hay nhap mat khau moi vao khung ben duoi.\n\n{D1D1D1}Vi ly do bao mat, hay dat mat khau {C75656}duy nhat {D1D1D1}ma ban chua tung su dung truoc day.";
            dialog.Button1 = "Dang ky";
            dialog.Button2 = "Thoat";
            dialog.IsPassword = true;
        }));
        await OnSignUpDialogResponse(player, response);
    }

    private async Task OnSignUpDialogResponse(Player player, InputDialogResponse response)
    {
        if (response.Response == DialogResponse.RightButtonOrCancel)
        {
            player.Kick();
            return;
        }

        var hash = await Task.Run(() => BCrypt.EnhancedHashPassword(response.InputText));

        AccountModel model = new AccountModel()
        {
            Name = player.Name,
            Password = hash
        };

        await using var context = await contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        await context.Accounts.AddAsync(model).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);

        player.AddComponent<AccountComponent>(model.Id);
        await signedUpEvent.InvokeAsync(player);
    }
}
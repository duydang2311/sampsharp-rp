using SampSharp.Entities.SAMP;

namespace Server.Account.Systems.SignUp;

using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Server.Account.Models;
using Server.Database;

public sealed class SignUpSystem : ISignUpSystem
{
    private readonly IDbContextFactory<ServerDbContext> contextFactory;
    private readonly IDialogService dialogService;
    public SignUpSystem(IDbContextFactory<ServerDbContext> contextFactory, IDialogService dialogService)
    {
        this.contextFactory = contextFactory;
        this.dialogService = dialogService;
    }

    public async Task SignUp(Player player)
    {
        await OnPasswordDialogResponse(player, await ShowPasswordDialog(player));
    }

    private Task<InputDialogResponse> ShowPasswordDialog(Player player)
    {
        var signUpDialog = new InputDialog()
        {
            Caption = "<INSERT_SERVER_NAME> - Dang ky tai khoan",
            Content = "{FFFFFF}Xin chao{7f9eba} " + player.Name + "{FFFFFF}, tai khoan nay chua duoc dang ky hay nhap mat khau moi vao khung ben duoi.\n\n{D1D1D1}Vi ly do bao mat, hay dat mat khau {C75656}duy nhat {D1D1D1}ma ban chua tung su dung truoc day.",
            Button1 = "Dang ky",
            Button2 = "Thoat",
            IsPassword = true
        };
        return dialogService.Show(player, signUpDialog);
    }

    private async Task OnPasswordDialogResponse(Player player, InputDialogResponse response)
    {
        if (response.Response == DialogResponse.RightButtonOrCancel)
        {
            player.Kick();
            return;
        }

        var hash = await Task.Run(() =>
        {
            return BCrypt.EnhancedHashPassword(response.InputText);
        });

        AccountModel model = new AccountModel() {
            Name = player.Name,
            Password = hash
        };

        await using var context = await contextFactory.CreateDbContextAsync();
        await context.Accounts.AddAsync(model);
        await context.SaveChangesAsync();
    }
}
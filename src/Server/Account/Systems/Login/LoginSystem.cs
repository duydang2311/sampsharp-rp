using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Components;
using Server.Account.Systems.Authentication;
using Server.Database;
using Server.SAMP.Dialog.Services;
using BC = BCrypt.Net.BCrypt;

namespace Server.Account.Systems.Login;

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

		authenticatedEvent.AddHandler((player, signedUp, e) =>
		{
			if (!signedUp)
			{
				return;
			}
			e.Cancel = true;
			ShowLoginDialog(player);
		});
	}

	private void ShowLoginDialog(Player player)
	{
		dialogService.ShowInput(
			player,
			b => b
				.SetCaption(t => t.Dialog_Account_Login_Caption)
				.SetContent(t => t.Dialog_Account_Login_Content, player.Name)
				.SetButton1(t => t.Dialog_Account_Login_Button1)
				.SetButton2(t => t.Dialog_Account_Login_Button2)
				.SetIsPassword(true),
			async response => await HandleLoginDialogResponse(player, response).ConfigureAwait(false));
	}

	private async Task HandleLoginDialogResponse(Player player, InputDialogResponse response)
	{
		if (response.Response != DialogResponse.LeftButton)
		{
			player.Kick();
			return;
		}
		using var context = contextFactory.CreateDbContext();
		var account = (await context
			.Accounts
			.Where(model => player.Name == model.Name)
			.Select(model => new { model.Password, model.Id })
			.FirstOrDefaultAsync()
			.ConfigureAwait(false))!;
		var success = await Task
			.Run(() => BC.EnhancedVerify(response.InputText, account.Password))
			.ConfigureAwait(false);

		if (success)
		{
			player.AddComponent(new AccountComponent() { Id = account.Id });
			await loginEvent.InvokeAsync(player).ConfigureAwait(false);
		}
		else
		{
			ShowLoginDialog(player);
		}
		return;
	}
}

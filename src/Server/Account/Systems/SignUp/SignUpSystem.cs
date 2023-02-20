using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Components;
using Server.Account.Models;
using Server.Account.Systems.Authentication;
using Server.Database;
using Server.SAMP.Dialog.Services;
using BC = BCrypt.Net.BCrypt;

namespace Server.Account.Systems.SignUp;

public sealed class SignUpSystem : ISystem
{
	private readonly IDbContextFactory<ServerDbContext> contextFactory;
	private readonly ICustomDialogService dialogService;
	private readonly ISignedUpEvent signedUpEvent;

	public SignUpSystem(IDbContextFactory<ServerDbContext> contextFactory, ICustomDialogService dialogService, IAuthenticatedEvent authenticatedEvent, ISignedUpEvent signedUpEvent)
	{
		this.contextFactory = contextFactory;
		this.dialogService = dialogService;
		this.signedUpEvent = signedUpEvent;

		authenticatedEvent.AddHandler((player, signedUp, e) =>
		{
			if (signedUp)
			{
				return;
			}
			e.Cancel = true;
			ShowSignUpDialog(player);
		});
	}

	public void ShowSignUpDialog(Player player)
	{
		dialogService.ShowInput(
			player,
			b => b
				.SetCaption(t => t.Dialog_Account_SignUp_Caption)
				.SetContent(t => t.Dialog_Account_SignUp_Content, player.Name)
				.SetButton1(t => t.Dialog_Account_SignUp_Button1)
				.SetButton2(t => t.Dialog_Account_SignUp_Button2)
				.SetIsPassword(true),
			async response => await HandleSignUpDialogResponse(player, response).ConfigureAwait(false));
	}

	private async Task HandleSignUpDialogResponse(Player player, InputDialogResponse response)
	{
		if (response.Response != DialogResponse.LeftButton)
		{
			player.Kick();
			return;
		}

		var name = player.Name;
		var hash = await Task
			.Run(() => BC.EnhancedHashPassword(response.InputText))
			.ConfigureAwait(false);
		var model = new AccountModel()
		{
			Name = name,
			Password = hash
		};

		using var context = contextFactory.CreateDbContext();
		await context.Accounts.AddAsync(model).ConfigureAwait(false);
		await context.SaveChangesAsync().ConfigureAwait(false);

		player.AddComponent(new AccountComponent { Id = model.Id });
		await signedUpEvent.InvokeAsync(player).ConfigureAwait(false);
	}
}

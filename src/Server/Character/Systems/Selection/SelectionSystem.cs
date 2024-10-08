using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Components;
using Server.Account.Systems.Authentication;
using Server.Account.Systems.Login;
using Server.Account.Systems.SignUp;
using Server.Character.Systems.Creation;
using Server.Database;
using Server.SAMP.Dialog.Services;

namespace Server.Character.Systems.Selection;

public sealed class SelectionSystem : ISystem
{
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly ICustomDialogService dialogService;
	private readonly ICharacterSelectedEvent characterSelectedEvent;
	private readonly IAuthenticationSystem authenticationSystem;

	public SelectionSystem(ILoginEvent loginEvent, ISignedUpEvent signedupEvent, IDbContextFactory<ServerDbContext> dbContextFactory, ICustomDialogService dialogService, ICharacterSelectedEvent characterSelectedEvent, ICharacterCreatedEvent characterCreatedEvent, IAuthenticationSystem authenticationSystem)
	{
		this.dbContextFactory = dbContextFactory;
		this.dialogService = dialogService;
		this.characterSelectedEvent = characterSelectedEvent;
		this.authenticationSystem = authenticationSystem;

		loginEvent.AddHandler(ShowCharacterSelectionDialog);
		signedupEvent.AddHandler(ShowCharacterSelectionDialog);
		characterCreatedEvent.AddHandler(ShowCharacterSelectionDialog);
	}

	private async Task ShowCharacterSelectionDialog(Player player)
	{
		var account = player.GetComponent<AccountComponent>();
		if (account is null)
		{
			await authenticationSystem.AuthenticateAsync(player).ConfigureAwait(false);
			return;
		}
		using var ctx = dbContextFactory.CreateDbContext();
		var models = await ctx.Characters
			.Where(m => m.AccountId == account.Id)
			.Select(m => new { m.Id, m.Name, m.Age })
			.OrderBy(m => m.Id)
			.Take(3)
			.ToArrayAsync();
		dialogService.ShowTablist(
			player,
			b =>
			{
				b
					.AddColumn(t => t.Dialog_Character_Selection_Header_Column1)
					.AddColumn(t => t.Dialog_Character_Selection_Header_Column2)
					.SetCaption(t => t.Dialog_Character_Selection_Caption)
					.SetButton1(t => t.Dialog_Character_Selection_Button1)
					.SetButton2(t => t.Dialog_Character_Selection_Button2);
				foreach (var model in models)
				{
					b
						.AddRow(b => b
							.Add(t => t.Dialog_Character_Selection_Row_Column1, model.Name)
							.Add(t => t.Dialog_Character_Selection_Row_Column2, model.Age))
						.WithTag(model.Id);
				}
				if (models.Length < 3)
				{
					b.AddRow(b => b
						.Add(t => t.Dialog_Character_Selection_RowNewChar_Column1)
						.Add(t => t.Dialog_Character_Selection_RowNewChar_Column2));
				}
			},
			async response => await HandleSelectionResponse(player, response).ConfigureAwait(false));
	}

	private Task HandleSelectionResponse(Player player, TablistDialogResponse response)
	{
		if (response.Response != DialogResponse.LeftButton)
		{
			player.Kick();
			return Task.CompletedTask;
		}
		return characterSelectedEvent.InvokeAsync(player, response.Item.Tag is null ? -1 : (long)response.Item.Tag);
	}
}

using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Components;
using Server.Account.Systems.Login;
using Server.Account.Systems.SignUp;
using Server.Database;
using Server.SAMP.Dialog.Services;

namespace Server.Character.Systems.Selection;

public sealed class SelectionSystem : ISystem
{
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly ICustomDialogService dialogService;
	private readonly ICharacterSelectedEvent characterSelectedEvent;

	public SelectionSystem(ILoginEvent loginEvent, ISignedUpEvent signedupEvent, IDbContextFactory<ServerDbContext> dbContextFactory, ICustomDialogService dialogService, ICharacterSelectedEvent characterSelectedEvent)
	{
		this.dbContextFactory = dbContextFactory;
		this.dialogService = dialogService;
		this.characterSelectedEvent = characterSelectedEvent;
		loginEvent.AddHandler(EnterCharacterSelection);
		signedupEvent.AddHandler(EnterCharacterSelection);
	}

	private async Task EnterCharacterSelection(Player player)
	{
		var account = player.GetComponent<AccountComponent>();
		if (account is null)
		{
			player.Kick();
			return;
		}
		await using var ctx = await dbContextFactory.CreateDbContextAsync();
		var models = await ctx.Characters
			.Where(m => m.AccountId == account.Id)
			.Select(m => new { m.Id, m.Name, m.Age })
			.OrderBy(m => m.Id)
			.Take(3)
			.ToArrayAsync();
		var response = await dialogService.ShowTablistAsync(player, b =>
		{
			b
				.AddColumn(t => t.Dialog_CharacterSelection_Header_Column1)
				.AddColumn(t => t.Dialog_CharacterSelection_Header_Column2)
				.SetCaption(t => t.Dialog_CharacterSelection_Caption)
				.SetButton1(t => t.Dialog_CharacterSelection_Button1)
				.SetButton2(t => t.Dialog_CharacterSelection_Button2);
			foreach (var model in models)
			{
				b
					.AddRow(b => b
						.Add(t => t.Dialog_CharacterSelection_Row_Column1, model.Name)
						.Add(t => t.Dialog_CharacterSelection_Row_Column2, model.Age))
					.WithTag(model.Id);
			}
			if (models.Length < 3)
			{
				b.AddRow(b => b
					.Add(t => t.Dialog_CharacterSelection_RowNewChar_Column1)
					.Add(t => t.Dialog_CharacterSelection_RowNewChar_Column2));
			}
		});
		await HandleSelectionResponse(player, response);
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

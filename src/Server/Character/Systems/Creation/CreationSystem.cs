using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Components;
using Server.Account.Systems.Authentication;
using Server.Account.Systems.Login;
using Server.Character.Models;
using Server.Character.Systems.Selection;
using Server.Database;
using Server.SAMP.Dialog.Services;

namespace Server.Character.Systems.Creation;

public sealed class CreationSystem : ISystem
{
	private sealed class CreationDataComponent : Component
	{
		public string Name { get; set; } = string.Empty;
		public bool IsMale { get; set; }
		public int Age { get; set; }
	}

	private readonly ICustomDialogService dialogService;
	private readonly IDbContextFactory<ServerDbContext> dbContextFactory;
	private readonly ICharacterCreatedEvent characterCreatedEvent;
	private readonly ILoginEvent loginEvent;
	private readonly IAuthenticationSystem authenticationSystem;

	public CreationSystem(ICharacterSelectedEvent characterSelectedEvent, ICustomDialogService dialogService, IDbContextFactory<ServerDbContext> dbContextFactory, ICharacterCreatedEvent characterCreatedEvent, ILoginEvent loginEvent, IAuthenticationSystem authenticationSystem)
	{
		this.dialogService = dialogService;
		this.dbContextFactory = dbContextFactory;
		this.characterCreatedEvent = characterCreatedEvent;
		this.loginEvent = loginEvent;
		this.authenticationSystem = authenticationSystem;

		characterSelectedEvent.AddHandler((player, id, e) =>
		{
			if (id != -1)
			{
				return;
			}
			e.Cancel = true;
			ShowCharacterGenderDialog(player);
		});
	}

	private void ShowCharacterGenderDialog(Player player)
	{
		dialogService.ShowTablist(
			player,
			b => b
				.SetCaption(t => t.Dialog_Character_Creation_Gender_Caption)
				.SetButton1(t => t.Dialog_Character_Creation_Gender_Button1)
				.SetButton2(t => t.Dialog_Character_Creation_Gender_Button2)
				.AddColumn(t => t.Dialog_Character_Creation_Gender_Header_Column1)
				.AddRow(b => b.Add(t => t.Dialog_Character_Creation_Gender_Row_Male))
				.AddRow(b => b.Add(t => t.Dialog_Character_Creation_Gender_Row_Female)),
			async (response) => await HandleCharacterGenderDialogResponse(player, response).ConfigureAwait(false));
	}

	private async Task HandleCharacterGenderDialogResponse(Player player, TablistDialogResponse response)
	{
		if (response.Response != DialogResponse.LeftButton)
		{
			await loginEvent.InvokeAsync(player);
			return;
		}
		var component = player.GetComponent<CreationDataComponent>();
		if (component is null)
		{
			component = new CreationDataComponent
			{
				IsMale = response.ItemIndex == 0
			};
			player.AddComponent(component);
		}
		ShowCharacterNameDialog(player);
	}

	private void ShowCharacterNameDialog(Player player)
	{
		dialogService.ShowInput(player, b => b
			.SetCaption(t => t.Dialog_Character_Creation_Name_Caption)
			.SetContent(t => t.Dialog_Character_Creation_Name_Content)
			.SetButton1(t => t.Dialog_Character_Creation_Name_Button1)
			.SetButton2(t => t.Dialog_Character_Creation_Name_Button2),
			response => HandleCharacterNameDialogResponse(player, response));
	}

	private void HandleCharacterNameDialogResponse(Player player, InputDialogResponse response)
	{
		if (response.Response != DialogResponse.LeftButton)
		{
			ShowCharacterGenderDialog(player);
			return;
		}

		var inputText = response.InputText.Trim();
		if (string.IsNullOrEmpty(inputText))
		{
			ShowCharacterNameDialog(player);
			return;
		}

		var regex = new Regex(@"[a-zA-Z]+_[a-zA-Z]+");
		if (!regex.IsMatch(inputText))
		{
			ShowCharacterNameDialog(player);
			return;
		}

		var component = player.GetComponent<CreationDataComponent>();
		if (component is null)
		{
			ShowCharacterNameDialog(player);
			return;
		}

		component.Name = inputText;
		ShowCharacterAgeDialog(player);
	}

	private void ShowCharacterAgeDialog(Player player)
	{
		dialogService.ShowInput(
			player,
			b => b
				.SetCaption(t => t.Dialog_Character_Creation_Age_Caption)
				.SetContent(t => t.Dialog_Character_Creation_Age_Content)
				.SetButton1(t => t.Dialog_Character_Creation_Age_Button1)
				.SetButton2(t => t.Dialog_Character_Creation_Age_Button2),
			async response => await HandleCharacterAgeDialogResponse(player, response).ConfigureAwait(false));
	}

	private async Task HandleCharacterAgeDialogResponse(Player player, InputDialogResponse response)
	{
		if (response.Response != DialogResponse.LeftButton)
		{
			ShowCharacterNameDialog(player);
			return;
		}

		var accountComponent = player.GetComponent<AccountComponent>();
		if (accountComponent is null)
		{
			await authenticationSystem.AuthenticateAsync(player);
			return;
		}

		var creationDataComponent = player.GetComponent<CreationDataComponent>();
		if (creationDataComponent is null)
		{
			ShowCharacterGenderDialog(player);
			return;
		}

		if (!int.TryParse(response.InputText, out var age) || age < 17 || age > 80)
		{
			ShowCharacterAgeDialog(player);
			return;
		}

		creationDataComponent.Age = age;
		await using var context = await dbContextFactory.CreateDbContextAsync().ConfigureAwait(false);

		var model = new CharacterModel()
		{
			AccountId = accountComponent.Id,
			Name = creationDataComponent.Name,
			Gender = creationDataComponent.IsMale,
			Age = creationDataComponent.Age
		};
		await context.Characters.AddAsync(model).ConfigureAwait(false);
		await context.SaveChangesAsync().ConfigureAwait(false);
		await characterCreatedEvent.InvokeAsync(player).ConfigureAwait(false);
	}
}

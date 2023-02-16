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
	private readonly IAuthenticatedEvent authenticatedEvent;

	public CreationSystem(ICharacterSelectedEvent characterSelectedEvent, ICustomDialogService dialogService, IDbContextFactory<ServerDbContext> dbContextFactory, ICharacterCreatedEvent characterCreatedEvent, ILoginEvent
			loginEvent, IAuthenticationSystem authenticationSystem, IAuthenticatedEvent authenticatedEvent)
	{
		this.dialogService = dialogService;
		this.dbContextFactory = dbContextFactory;
		this.characterCreatedEvent = characterCreatedEvent;
		this.loginEvent = loginEvent;
		this.authenticationSystem = authenticationSystem;
		this.authenticatedEvent = authenticatedEvent;

		characterSelectedEvent.AddHandler((player, id, e) =>
		{
			if (id != -1)
			{
				return Task.CompletedTask;
			}
			e.Cancel = true;
			return ShowCharacterGenderDialog(player);
		});
	}

	private async Task ShowCharacterGenderDialog(Player player)
	{
		var response = await dialogService.ShowTablistAsync(player, b => b
			.SetCaption(t => t.Dialog_Character_Creation_Gender_Caption)
			.SetButton1(t => t.Dialog_Character_Creation_Gender_Button1)
			.SetButton2(t => t.Dialog_Character_Creation_Gender_Button2)
			.AddColumn(t => t.Dialog_Character_Creation_Gender_Header_Column1)
			.AddRow(b => b.Add(t => t.Dialog_Character_Creation_Gender_Row_Male))
			.AddRow(b => b.Add(t => t.Dialog_Character_Creation_Gender_Row_Female)));
		await HandleCharacterGenderDialogResponse(player, response);
	}

	private async Task HandleCharacterGenderDialogResponse(Player player, TablistDialogResponse response)
	{
		if (response.Response != DialogResponse.LeftButton)
		{
			await Task.Yield();
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
		await ShowCharacterNameDialog(player);
	}

	private async Task ShowCharacterNameDialog(Player player)
	{
		var response = await dialogService.ShowInputAsync(player, b => b
			.SetCaption(t => t.Dialog_Character_Creation_Name_Caption)
			.SetContent(t => t.Dialog_Character_Creation_Name_Content)
			.SetButton1(t => t.Dialog_Character_Creation_Name_Button1)
			.SetButton2(t => t.Dialog_Character_Creation_Name_Button2));
		await HandleCharacterNameDialogResponse(player, response);
	}

	private async Task HandleCharacterNameDialogResponse(Player player, InputDialogResponse response)
	{
		var inputText = response.InputText.Trim();
		if (string.IsNullOrEmpty(inputText))
		{
			await Task.Yield();
			await ShowCharacterNameDialog(player);
			return;
		}

		var regex = new Regex(@"[a-zA-Z]+_[a-zA-Z]+");
		if (!regex.IsMatch(inputText))
		{
			await Task.Yield();
			await ShowCharacterNameDialog(player);
			return;
		}

		var component = player.GetComponent<CreationDataComponent>();
		if (component is null)
		{
			await Task.Yield();
			await ShowCharacterNameDialog(player);
			return;
		}

		component.Name = inputText;
		await ShowCharacterAgeDialog(player);
	}

	private async Task ShowCharacterAgeDialog(Player player)
	{
		var response = await dialogService.ShowInputAsync(player, b => b
			.SetCaption(t => t.Dialog_Character_Creation_Age_Caption)
			.SetContent(t => t.Dialog_Character_Creation_Age_Content)
			.SetButton1(t => t.Dialog_Character_Creation_Age_Button1)
			.SetButton2(t => t.Dialog_Character_Creation_Age_Button2)
		);
		await HandleCharacterAgeDialogResponse(player, response);
	}

	private async Task HandleCharacterAgeDialogResponse(Player player, InputDialogResponse response)
	{
		if (response.Response != DialogResponse.LeftButton)
		{
			await Task.Yield();
			await ShowCharacterNameDialog(player);
			return;
		}

		var accountComponent = player.GetComponent<AccountComponent>();
		if (accountComponent is null)
		{
			await Task.Yield();
			await authenticatedEvent.InvokeAsync(player, await authenticationSystem.IsAccountSignedUpAsync(player));
			return;
		}

		var creationDataComponent = player.GetComponent<CreationDataComponent>();
		if (creationDataComponent is null)
		{
			await Task.Yield();
			await ShowCharacterGenderDialog(player);
			return;
		}

		if (!int.TryParse(response.InputText, out var age) || age < 17 || age > 80)
		{
			await Task.Yield();
			await ShowCharacterAgeDialog(player);
			return;
		}

		creationDataComponent.Age = age;
		await using var context = await dbContextFactory.CreateDbContextAsync();

		var model = new CharacterModel()
		{
			AccountId = accountComponent.Id,
			Name = creationDataComponent.Name,
			Gender = creationDataComponent.IsMale,
			Age = creationDataComponent.Age
		};
		await context.Characters.AddAsync(model).ConfigureAwait(false);
		await context.SaveChangesAsync().ConfigureAwait(false);
		await characterCreatedEvent.InvokeAsync(player);
	}
}

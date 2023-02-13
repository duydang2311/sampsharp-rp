using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using SampSharp.Core;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.Account.Components;
using Server.Character.Systems.Selection;
using Server.Chat.Services;
using Server.Database;

namespace Server.Character.Systems.Creation;

using BCrypt.Net;
using Models;

public class CreationSystem : ISystem
{
    private class CreationData
    {
        public long AccountId;
        public string Name = string.Empty;
        public bool Gender;
        public int Age;
    }

    private readonly IDbContextFactory<ServerDbContext> contextFactory;
    private readonly IDialogService dialogService;
    private readonly ICreationFinishedEvent creationFinishedEvent;
    private readonly ISynchronizationProvider syncProvider;
    private readonly IChatService chatService;
    private readonly IDictionary<Player, CreationData> dataDict = new Dictionary<Player, CreationData>();

    public CreationSystem(IDbContextFactory<ServerDbContext> contextFactory, IDialogService dialogService,
        ICreationFinishedEvent creationFinishedEvent, ISynchronizationProvider syncProvider,
        ISelectionRequestSetupEvent selectionRequestSetupEvent, IChatService chatService)
    {
        this.dialogService = dialogService;
        this.contextFactory = contextFactory;
        this.creationFinishedEvent = creationFinishedEvent;
        this.syncProvider = syncProvider;
        this.chatService = chatService;
        selectionRequestSetupEvent.AddHandler(OnRequestSetupAsync);
    }

    public async Task OnRequestSetupAsync(Player player)
    {
        var account = player.GetComponent<AccountComponent>();
        dataDict.Add(player, new CreationData()
        {
            AccountId = account.Id
        });
        await OnNameDialogResponse(player, await ShowNameDialog(player));
    }

    private Task<InputDialogResponse> ShowNameDialog(Player player)
    {
        var nameDialog = new InputDialog()
        {
            Caption = "Ten nhan vat",
            Content = "Ten nhan vat cua ban se la?\n\n* Ten nhan vat phai theo cu phap Ho_Ten hoac Ten_Ho.",
            Button1 = "Tiep tuc",
            Button2 = "Quay lai"
        };

        return dialogService.Show(player, nameDialog);
    }

    private async Task OnNameDialogResponse(Player player, InputDialogResponse response)
    {
        if (response.Response != DialogResponse.LeftButton)
        {
            await ShowNameDialog(player);
            return;
        }

        if (response.Response != DialogResponse.LeftButton)
        {
            await ShowGenderDialog(player);
            return;
        }

        if (!dataDict.TryGetValue(player, out var value))
        {
            await ShowGenderDialog(player);
            return;
        }

        value.Name = response.InputText;

        await OnGenderDialogResponse(player, await ShowGenderDialog(player));
    }

    private Task<TablistDialogResponse> ShowGenderDialog(Player player)
    {
        var genderDialog = new TablistDialog("Gioi tinh nhan vat", "Tiep tuc", "Quay lai",
            new[] { "Nhan vat cua ban gioi tinh se la?" })
        {
            "Nam",
            "Nu"
        };
        return dialogService.Show(player, genderDialog);
    }

    private async Task OnGenderDialogResponse(Player player, TablistDialogResponse response)
    {
        if (response.Response != DialogResponse.LeftButton)
        {
            await ShowNameDialog(player);
            return;
        }

        if (!dataDict.TryGetValue(player, out var value))
        {
            await ShowGenderDialog(player);
            return;
        }

        value.Gender = response.ItemIndex == 1;

        await OnAgeDialogResponse(player, await ShowAgeDialog(player));
    }

    private Task<InputDialogResponse> ShowAgeDialog(Player player)
    {
        var ageDialog = new InputDialog()
        {
            Caption = "Tuoi nhan vat",
            Content = "Nhan vat cua ban bao nhieu tuoi? (17 tuoi - 80 tuoi)",
            Button1 = "Tiep tuc",
            Button2 = "Quay lai"
        };

        return dialogService.Show(player, ageDialog);
    }

    private async Task OnAgeDialogResponse(Player player, InputDialogResponse response)
    {
        if (response.Response != DialogResponse.LeftButton)
        {
            await ShowGenderDialog(player);
            return;
        }

        if (!dataDict.TryGetValue(player, out var value))
        {
            await ShowGenderDialog(player);
            return;
        }

        int age;
        var result = int.TryParse(response.InputText, out age);

        if (result == false || age < 17 || age > 80)
        {
            chatService.SendMessage(player, b => b.Add(Color.Tomato, m => m.Character_Creation_AgeError));
            await ShowAgeDialog(player);
            return;
        }

        value.Age = age;

        await using var context = await contextFactory.CreateDbContextAsync();

        var model = new CharacterModel()
        {
            AccountId = value.AccountId,
            Name = value.Name,
            Gender = value.Gender,
            Age = value.Age
        };
        await context.Characters.AddAsync(model).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);
        await creationFinishedEvent.InvokeAsync(player);
        syncProvider.Invoke(() =>
        {
            creationFinishedEvent
                .InvokeAsync(player)
                .ContinueWith((t) => throw t.Exception!, TaskContinuationOptions.OnlyOnFaulted);
        });
    }
}
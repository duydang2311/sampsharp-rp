using System.Globalization;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using Server.I18N.Localization.Components;

namespace Server.SAMP.Dialog.Services;

public sealed class CustomDialogService : ICustomDialogService
{
	private readonly IDialogBuilderFactory dialogBuilderFactory;
	private readonly IDialogService sampSharpDialogService;

	public CustomDialogService(IDialogBuilderFactory dialogBuilderFactory, IDialogService sampSharpDialogService)
	{
		this.dialogBuilderFactory = dialogBuilderFactory;
		this.sampSharpDialogService = sampSharpDialogService;
	}

	public Task<MessageDialogResponse> ShowMessageAsync(Player player, Action<IMessageDialogBuilder> buildDialog)
	{
		var builder = dialogBuilderFactory.CreateMessageBuilder();
		buildDialog(builder);
		return ShowAsync<MessageDialogResponse, MessageDialog, IMessageDialogBuilder>(player, builder);
	}

	public Task<InputDialogResponse> ShowInputAsync(Player player, Action<IInputDialogBuilder> buildDialog)
	{
		var builder = dialogBuilderFactory.CreateInputBuilder();
		buildDialog(builder);
		return ShowAsync<InputDialogResponse, InputDialog, IInputDialogBuilder>(player, builder);
	}

	public Task<ListDialogResponse> ShowListAsync(Player player, Action<IListDialogBuilder> buildDialog)
	{
		var builder = dialogBuilderFactory.CreateListBuilder();
		buildDialog(builder);
		return ShowAsync<ListDialogResponse, ListDialog, IListDialogBuilder>(player, builder);
	}

	public Task<TablistDialogResponse> ShowTablistAsync(Player player, Action<ITablistDialogBuilder> buildDialog)
	{
		var builder = dialogBuilderFactory.CreateTablistBuilder();
		buildDialog(builder);
		return ShowAsync<TablistDialogResponse, TablistDialog, ITablistDialogBuilder>(player, builder);
	}

	public Task<TResponse> ShowAsync<TResponse>(EntityId player, IDialog<TResponse> dialog)
	where TResponse : struct
	{
		return sampSharpDialogService.ShowAsync(player, dialog);
	}

	private Task<TResponse> ShowAsync<TResponse, TDialog, TBuilder>(Player player, IDialogBuilder<TDialog, TBuilder> builder)
	where TResponse : struct
	where TDialog : IDialog<TResponse>
	where TBuilder : IDialogBuilder<TDialog, TBuilder>
	{
		var cultureInfo = player.GetComponent<CultureComponent>()?.Culture ?? CultureInfo.InvariantCulture;
		return ShowAsync(player, builder.Build(cultureInfo));
	}
}

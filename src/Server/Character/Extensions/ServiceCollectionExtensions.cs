using SampSharp.Entities;
using Server.Character.Systems.Creation;
using Server.Character.Systems.EnterCommand;
using Server.Character.Systems.RolePlayCommands;
using Server.Character.Systems.Selection;
using Server.Character.Systems.Spawn;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection WithCharacter(this IServiceCollection self)
	{
		return self
			.AddSystem<AmeCommandSystem>()
			.AddSystem<ShoutCommandSystem>()
			.AddSystem<MeCommandSystem>()
			.AddSystem<LowCommandSystem>()
			.AddSystem<DoCommandSystem>()
			.AddSystem<EnterCommandSystem>()
			.AddSystem<SelectionSystem>()
			.AddSystem<CreationSystem>()
			.AddSystem<SpawnSystem>()
			.AddSingleton<IEnterCommandEvent, EnterCommandEvent>()
			.AddSingleton<ICharacterSelectedEvent, CharacterSelectedEvent>()
			.AddSingleton<ICharacterCreatedEvent, CharacterCreatedEvent>()
			.AddSingleton<ICharacterSpawnedEvent, CharacterSpawnedEvent>();
	}
}

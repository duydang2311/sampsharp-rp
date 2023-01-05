using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using SampSharp.Entities.SAMP.Commands;

namespace Server.Chat.Services;

public class CustomPlayerCommandService : PlayerCommandService, ICustomPlayerCommandService
{
	public CustomPlayerCommandService(IEntityManager entityManager) : base(entityManager) { }

	public new InvokeResult Invoke(IServiceProvider services, EntityId player, string inputText)
	{
		if (!player.IsOfType(SampEntities.PlayerType))
			throw new InvalidEntityArgumentException(nameof(player), SampEntities.PlayerType);
		return Invoke(services, new object[] { player }, inputText);
	}

	protected override string GetUsageMessage(CommandInfo[] commands)
	{
		return "Hướng dẫn" + base.GetUsageMessage(commands).Remove(0, "Usage".Length);
	}
}

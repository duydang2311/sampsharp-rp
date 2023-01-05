using SampSharp.Entities;
using SampSharp.Entities.SAMP.Commands;

namespace Server.Chat.Services;

public interface ICustomPlayerCommandService
{
	InvokeResult Invoke(IServiceProvider services, EntityId player, string inputText);
}

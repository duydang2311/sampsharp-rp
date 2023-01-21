using SampSharp.Entities;

namespace Server.Account.Components;

public sealed class AccountComponent : Component
{
    public long Id { get; set; }

    public AccountComponent(long Id)
    {
        this.Id = Id;
    }
}
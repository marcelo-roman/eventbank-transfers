using EventBank.BuildingBlocks.Domain;

namespace EventBank.Transfers.Domain.Pix;

public sealed class PixKey : AggregateRoot<PixKeyId>
{
    public const int KeysPerAccount = 5;

    private PixKey()
    {
    }

    public PixKeyValue Key { get; private set; } = null!;

    public AccountId Account { get; private set; }

    public Guid OwnerId { get; private set; }

    public PixKeyStatus Status { get; private set; }

    public static PixKey Register(PixKeyValue key, AccountReference account, PixKeyOwner owner, int keysOnAccount)
    {
        var pixKey = new PixKey();
        pixKey.Claim(key, account, owner, keysOnAccount);
        return pixKey;
    }

    public void Reclaim(AccountReference account, PixKeyOwner owner, int keysOnAccount)
    {
        if (Status == PixKeyStatus.Active)
        {
            throw PixErrors.KeyInUse();
        }

        Claim(Key, account, owner, keysOnAccount);
    }

    public void Remove(Guid requestedBy)
    {
        if (Status != PixKeyStatus.Active)
        {
            throw PixErrors.KeyNotRegistered();
        }

        if (OwnerId != requestedBy)
        {
            throw PixErrors.KeyNotYours();
        }

        Raise(new PixKeyRemoved(Id.Value, Key.Value, OwnerId));
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case PixKeyRegistered registered:
                Id = new PixKeyId(registered.PixKeyId);
                Key = PixKeyValue.Restore(Enum.Parse<PixKeyType>(registered.Type), registered.Key);
                Account = new AccountId(registered.AccountId);
                OwnerId = registered.OwnerId;
                Status = PixKeyStatus.Active;
                break;
            case PixKeyRemoved:
                Status = PixKeyStatus.Removed;
                break;
            default:
                throw new InvalidOperationException($"{nameof(PixKey)} cannot apply {domainEvent.GetType().Name}.");
        }
    }

    private void Claim(PixKeyValue key, AccountReference account, PixKeyOwner owner, int keysOnAccount)
    {
        if (account.OwnerId != owner.CustomerId)
        {
            throw PixErrors.AccountNotYours();
        }

        if (!account.IsOpen)
        {
            throw PixErrors.AccountClosed();
        }

        if (keysOnAccount >= KeysPerAccount)
        {
            throw PixErrors.TooManyKeys(KeysPerAccount);
        }

        if ((key.Type == PixKeyType.Cpf && key.Value != owner.Cpf) || (key.Type == PixKeyType.Email && key.Value != owner.Email))
        {
            throw PixErrors.KeyNotYours();
        }

        Raise(new PixKeyRegistered(PixKeyId.For(key).Value, key.Type.ToString(), key.Value, account.Id.Value, owner.CustomerId, owner.FullName));
    }
}

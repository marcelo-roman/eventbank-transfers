namespace EventBank.Transfers.Application.Pix;

public interface IPixKeyReadStore
{
    Task<ActivePixKey?> FindActive(string key, CancellationToken cancellationToken);

    Task<int> CountActiveOnAccount(Guid accountId, CancellationToken cancellationToken);

    Task<IReadOnlyList<ActivePixKey>> ListActiveOf(Guid ownerId, CancellationToken cancellationToken);
}

public sealed record ActivePixKey(string Key, string Type, Guid AccountId, Guid OwnerId, string OwnerName, DateTimeOffset RegisteredAt);

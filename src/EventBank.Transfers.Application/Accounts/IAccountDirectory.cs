using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Accounts;

public interface IAccountDirectory
{
    Task<AccountReference?> Find(AccountId accountId, CancellationToken cancellationToken);

    Task<IReadOnlyList<Guid>> AccountIdsOf(Guid ownerId, CancellationToken cancellationToken);

    Task Register(AccountId accountId, Guid ownerId, string holder, string currency, string number, CancellationToken cancellationToken);

    Task MarkClosed(AccountId accountId, CancellationToken cancellationToken);
}

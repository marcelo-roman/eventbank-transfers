using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Accounts;

public interface IAccountDirectory
{
    Task<AccountReference?> Find(AccountId accountId, CancellationToken cancellationToken);

    Task Register(AccountId accountId, string holder, string currency, CancellationToken cancellationToken);

    Task MarkClosed(AccountId accountId, CancellationToken cancellationToken);
}

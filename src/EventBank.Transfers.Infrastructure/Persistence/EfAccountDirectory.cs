using EventBank.Transfers.Application.Accounts;
using EventBank.Transfers.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class EfAccountDirectory(TransfersDbContext dbContext) : IAccountDirectory
{
    public async Task<AccountReference?> Find(AccountId accountId, CancellationToken cancellationToken)
    {
        var entry = await dbContext.AccountDirectory
            .AsNoTracking()
            .FirstOrDefaultAsync(account => account.AccountId == accountId.Value, cancellationToken);

        return entry is null ? null : new AccountReference(accountId, Currency.From(entry.Currency), entry.IsOpen);
    }

    public async Task Register(AccountId accountId, string holder, string currency, CancellationToken cancellationToken)
    {
        var known = await dbContext.AccountDirectory.AnyAsync(account => account.AccountId == accountId.Value, cancellationToken);
        if (known)
        {
            return;
        }

        dbContext.AccountDirectory.Add(new AccountDirectoryEntry
        {
            AccountId = accountId.Value,
            Holder = holder,
            Currency = currency,
            IsOpen = true,
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task MarkClosed(AccountId accountId, CancellationToken cancellationToken) =>
        dbContext.AccountDirectory
            .Where(account => account.AccountId == accountId.Value)
            .ExecuteUpdateAsync(setters => setters.SetProperty(account => account.IsOpen, false), cancellationToken);
}

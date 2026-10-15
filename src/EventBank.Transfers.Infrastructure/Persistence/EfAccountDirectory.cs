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

        return entry is null ? null : new AccountReference(accountId, entry.OwnerId, Currency.From(entry.Currency), entry.IsOpen);
    }

    public async Task<IReadOnlyList<Guid>> AccountIdsOf(Guid ownerId, CancellationToken cancellationToken) =>
        await dbContext.AccountDirectory
            .Where(account => account.OwnerId == ownerId)
            .Select(account => account.AccountId)
            .ToListAsync(cancellationToken);

    public async Task Register(
        AccountId accountId,
        Guid ownerId,
        string holder,
        string currency,
        string number,
        CancellationToken cancellationToken)
    {
        if (await dbContext.AccountDirectory.AnyAsync(account => account.AccountId == accountId.Value, cancellationToken))
        {
            return;
        }

        dbContext.AccountDirectory.Add(new AccountDirectoryEntry
        {
            AccountId = accountId.Value,
            OwnerId = ownerId,
            Holder = holder,
            Currency = currency,
            Number = number,
            IsOpen = true,
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task MarkClosed(AccountId accountId, CancellationToken cancellationToken) =>
        dbContext.AccountDirectory
            .Where(account => account.AccountId == accountId.Value)
            .ExecuteUpdateAsync(setters => setters.SetProperty(account => account.IsOpen, false), cancellationToken);
}

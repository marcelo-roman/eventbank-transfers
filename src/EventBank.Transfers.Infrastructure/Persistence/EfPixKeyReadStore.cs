using EventBank.Transfers.Application.Pix;
using Microsoft.EntityFrameworkCore;

namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class EfPixKeyReadStore(TransfersDbContext dbContext) : IPixKeyReadStore
{
    public Task<ActivePixKey?> FindActive(string key, CancellationToken cancellationToken) =>
        Views(Active().Where(entry => entry.Key == key)).FirstOrDefaultAsync(cancellationToken);

    public Task<int> CountActiveOnAccount(Guid accountId, CancellationToken cancellationToken) =>
        Active().CountAsync(entry => entry.AccountId == accountId, cancellationToken);

    public async Task<IReadOnlyList<ActivePixKey>> ListActiveOf(Guid ownerId, CancellationToken cancellationToken) =>
        await Views(Active().Where(entry => entry.OwnerId == ownerId).OrderBy(entry => entry.RegisteredAt)).ToListAsync(cancellationToken);

    private IQueryable<PixKeySummary> Active() => dbContext.PixKeys.AsNoTracking().Where(entry => entry.IsActive);

    private static IQueryable<ActivePixKey> Views(IQueryable<PixKeySummary> keys) =>
        keys.Select(entry => new ActivePixKey(entry.Key, entry.Type, entry.AccountId, entry.OwnerId, entry.OwnerName, entry.RegisteredAt));
}

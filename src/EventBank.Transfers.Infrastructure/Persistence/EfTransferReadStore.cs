using EventBank.Transfers.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class EfTransferReadStore(TransfersDbContext dbContext) : ITransferReadStore
{
    private const int PageSize = 100;

    public async Task<IReadOnlyList<TransferView>> ListTransfers(Guid? accountId, CancellationToken cancellationToken)
    {
        var transfers = dbContext.TransferSummaries.AsNoTracking();
        if (accountId is { } id)
        {
            transfers = transfers.Where(transfer => transfer.SourceAccountId == id || transfer.DestinationAccountId == id);
        }

        return await Views(transfers.OrderByDescending(transfer => transfer.RequestedAt).Take(PageSize))
            .ToListAsync(cancellationToken);
    }

    public Task<TransferView?> FindTransfer(Guid transferId, CancellationToken cancellationToken) =>
        Views(dbContext.TransferSummaries.AsNoTracking().Where(transfer => transfer.Id == transferId))
            .FirstOrDefaultAsync(cancellationToken);

    private static IQueryable<TransferView> Views(IQueryable<TransferSummary> transfers) =>
        transfers.Select(transfer => new TransferView(
            transfer.Id,
            transfer.SourceAccountId,
            transfer.SourceHolder,
            transfer.DestinationAccountId,
            transfer.DestinationHolder,
            transfer.Amount,
            transfer.Currency,
            transfer.Status,
            transfer.FailureReason,
            transfer.Version,
            transfer.RequestedAt,
            transfer.UpdatedAt));
}

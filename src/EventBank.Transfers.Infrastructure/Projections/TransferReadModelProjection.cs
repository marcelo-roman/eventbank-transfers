using EventBank.BuildingBlocks.EventSourcing.Projections;
using EventBank.BuildingBlocks.EventSourcing.Store;
using EventBank.Transfers.Domain;
using EventBank.Transfers.Domain.Events;
using EventBank.Transfers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventBank.Transfers.Infrastructure.Projections;

public sealed class TransferReadModelProjection : IProjection<TransfersDbContext>
{
    private const string UnknownHolder = "Unknown account";

    public async Task Project(EventEnvelope envelope, TransfersDbContext dbContext, CancellationToken cancellationToken)
    {
        if (envelope.Event is TransferRequested requested)
        {
            dbContext.TransferSummaries.Add(new TransferSummary
            {
                Id = requested.TransferId,
                SourceAccountId = requested.SourceAccountId,
                SourceHolder = await HolderOf(dbContext, requested.SourceAccountId, cancellationToken),
                DestinationAccountId = requested.DestinationAccountId,
                DestinationHolder = await HolderOf(dbContext, requested.DestinationAccountId, cancellationToken),
                Amount = requested.Amount,
                Currency = requested.Currency,
                Status = nameof(TransferStatus.Requested),
                Version = envelope.StreamVersion,
                RequestedAt = envelope.RecordedAt,
                UpdatedAt = envelope.RecordedAt,
            });
            return;
        }

        if (envelope.Event is not ITransferEvent transferEvent)
        {
            return;
        }

        var summary = await dbContext.TransferSummaries.FindAsync([transferEvent.TransferId], cancellationToken)
            ?? throw new InvalidOperationException($"Transfer summary {transferEvent.TransferId} is missing.");

        summary.Status = StatusAfter(transferEvent).ToString();
        summary.FailureReason = FailureReasonOf(transferEvent) ?? summary.FailureReason;
        summary.Version = envelope.StreamVersion;
        summary.UpdatedAt = envelope.RecordedAt;
    }

    public Task Reset(TransfersDbContext dbContext, CancellationToken cancellationToken) =>
        dbContext.TransferSummaries.ExecuteDeleteAsync(cancellationToken);

    private static async Task<string> HolderOf(TransfersDbContext dbContext, Guid accountId, CancellationToken cancellationToken)
    {
        var account = await dbContext.AccountDirectory.FindAsync([accountId], cancellationToken);
        return account?.Holder ?? UnknownHolder;
    }

    private static TransferStatus StatusAfter(ITransferEvent transferEvent) => transferEvent switch
    {
        TransferDebited => TransferStatus.Debited,
        TransferFailed => TransferStatus.Failed,
        TransferCompleted => TransferStatus.Completed,
        TransferCreditDeclined => TransferStatus.Reversing,
        TransferReversed => TransferStatus.Reversed,
        _ => throw new InvalidOperationException($"Unexpected transfer event {transferEvent.GetType().Name}."),
    };

    private static string? FailureReasonOf(ITransferEvent transferEvent) => transferEvent switch
    {
        TransferFailed failed => failed.Reason,
        TransferCreditDeclined declined => declined.Reason,
        _ => null,
    };
}

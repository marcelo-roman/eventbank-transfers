using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Queries;

public sealed class GetTransferEventsHandler(IAggregateHistory<Transfer, TransferId> history, TransferAccess access)
    : IQueryHandler<GetTransferEvents, IReadOnlyList<RecordedEvent>>
{
    public async Task<IReadOnlyList<RecordedEvent>> Handle(GetTransferEvents query, CancellationToken cancellationToken)
    {
        await access.EnsureCanRead(query.TransferId, cancellationToken);
        return await history.ReadEvents(new TransferId(query.TransferId), cancellationToken);
    }
}

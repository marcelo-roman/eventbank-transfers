using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Queries;

public sealed class GetTransferEventsHandler(IAggregateHistory<Transfer, TransferId> history)
    : IQueryHandler<GetTransferEvents, IReadOnlyList<RecordedEvent>>
{
    public Task<IReadOnlyList<RecordedEvent>> Handle(GetTransferEvents query, CancellationToken cancellationToken) =>
        history.ReadEvents(new TransferId(query.TransferId), cancellationToken);
}

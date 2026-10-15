using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;

namespace EventBank.Transfers.Application.Queries;

public sealed record GetTransferEvents(Guid TransferId) : IQuery<IReadOnlyList<RecordedEvent>>;

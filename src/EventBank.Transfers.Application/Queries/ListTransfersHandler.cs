using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Queries;

public sealed class ListTransfersHandler(ITransferReadStore readStore)
    : IQueryHandler<ListTransfers, IReadOnlyList<TransferView>>
{
    public Task<IReadOnlyList<TransferView>> Handle(ListTransfers query, CancellationToken cancellationToken) =>
        readStore.ListTransfers(query.AccountId, cancellationToken);
}

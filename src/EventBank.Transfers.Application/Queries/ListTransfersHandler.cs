using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Queries;

public sealed class ListTransfersHandler(ITransferReadStore readStore, TransferAccess access)
    : IQueryHandler<ListTransfers, IReadOnlyList<TransferView>>
{
    public async Task<IReadOnlyList<TransferView>> Handle(ListTransfers query, CancellationToken cancellationToken) =>
        await readStore.ListTransfers(await access.VisibleAccounts(cancellationToken), cancellationToken);
}

using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Queries;

public sealed class GetTransferHandler(ITransferReadStore readStore) : IQueryHandler<GetTransfer, TransferView?>
{
    public Task<TransferView?> Handle(GetTransfer query, CancellationToken cancellationToken) =>
        readStore.FindTransfer(query.TransferId, cancellationToken);
}

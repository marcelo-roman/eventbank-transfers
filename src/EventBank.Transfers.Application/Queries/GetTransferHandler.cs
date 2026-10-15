using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Queries;

public sealed class GetTransferHandler(TransferAccess access) : IQueryHandler<GetTransfer, TransferView?>
{
    public async Task<TransferView?> Handle(GetTransfer query, CancellationToken cancellationToken) =>
        await access.EnsureCanRead(query.TransferId, cancellationToken);
}

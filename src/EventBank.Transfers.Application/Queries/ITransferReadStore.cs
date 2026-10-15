namespace EventBank.Transfers.Application.Queries;

public interface ITransferReadStore
{
    Task<IReadOnlyList<TransferView>> ListTransfers(IReadOnlyCollection<Guid>? accountIds, CancellationToken cancellationToken);

    Task<TransferView?> FindTransfer(Guid transferId, CancellationToken cancellationToken);
}

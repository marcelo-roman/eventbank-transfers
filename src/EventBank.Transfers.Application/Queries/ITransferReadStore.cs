namespace EventBank.Transfers.Application.Queries;

public interface ITransferReadStore
{
    Task<IReadOnlyList<TransferView>> ListTransfers(Guid? accountId, CancellationToken cancellationToken);

    Task<TransferView?> FindTransfer(Guid transferId, CancellationToken cancellationToken);
}

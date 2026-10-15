using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.BuildingBlocks.Application.Security;
using EventBank.Transfers.Application.Accounts;

namespace EventBank.Transfers.Application.Queries;

public sealed class TransferAccess(ICurrentUser currentUser, IAccountDirectory directory, ITransferReadStore readStore)
{
    public async Task<IReadOnlyCollection<Guid>?> VisibleAccounts(CancellationToken cancellationToken) =>
        currentUser.IsOperator ? null : await directory.AccountIdsOf(currentUser.Id, cancellationToken);

    public async Task<TransferView> EnsureCanRead(Guid transferId, CancellationToken cancellationToken)
    {
        var transfer = await readStore.FindTransfer(transferId, cancellationToken)
            ?? throw new NotFoundException("Transfer", transferId);
        var visible = await VisibleAccounts(cancellationToken);
        if (visible is not null && !visible.Contains(transfer.SourceAccountId) && !visible.Contains(transfer.DestinationAccountId))
        {
            throw new ForbiddenException("This transfer does not involve your accounts.");
        }

        return transfer;
    }
}

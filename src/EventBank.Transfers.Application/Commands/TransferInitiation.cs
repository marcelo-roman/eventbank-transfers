using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.BuildingBlocks.Application.Security;
using EventBank.Transfers.Application.Accounts;
using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Commands;

public sealed class TransferInitiation(
    IEventSourcedRepository<Transfer, TransferId> transfers,
    IAccountDirectory directory,
    ICurrentUser currentUser)
{
    public async Task<CommandResult> Start(Guid sourceAccountId, Guid destinationAccountId, decimal amount, CancellationToken cancellationToken)
    {
        var source = await FindAccount(sourceAccountId, cancellationToken);
        if (source.OwnerId != currentUser.Id)
        {
            throw new ForbiddenException("Transfers can only leave your own accounts.");
        }

        var destination = await FindAccount(destinationAccountId, cancellationToken);
        var transfer = Transfer.Request(TransferId.New(), source, destination, Money.Of(amount, source.Currency));
        await transfers.Save(transfer, cancellationToken);
        return new CommandResult(transfer.Id.Value, transfer.Version);
    }

    private async Task<AccountReference> FindAccount(Guid accountId, CancellationToken cancellationToken) =>
        await directory.Find(new AccountId(accountId), cancellationToken)
            ?? throw new NotFoundException("Account", accountId);
}

using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.Transfers.Application.Accounts;
using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Commands;

public sealed class RequestTransferHandler(
    IEventSourcedRepository<Transfer, TransferId> transfers,
    IAccountDirectory directory) : ICommandHandler<RequestTransfer, CommandResult>
{
    public async Task<CommandResult> Handle(RequestTransfer command, CancellationToken cancellationToken)
    {
        var source = await FindAccount(command.SourceAccountId, cancellationToken);
        var destination = await FindAccount(command.DestinationAccountId, cancellationToken);

        var transfer = Transfer.Request(TransferId.New(), source, destination, Money.Of(command.Amount, source.Currency));
        await transfers.Save(transfer, cancellationToken);
        return new CommandResult(transfer.Id.Value, transfer.Version);
    }

    private async Task<AccountReference> FindAccount(Guid accountId, CancellationToken cancellationToken) =>
        await directory.Find(new AccountId(accountId), cancellationToken)
            ?? throw new NotFoundException("Account", accountId);
}

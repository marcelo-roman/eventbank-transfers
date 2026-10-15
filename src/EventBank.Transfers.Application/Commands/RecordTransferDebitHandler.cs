using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Commands;

public sealed class RecordTransferDebitHandler(IEventSourcedRepository<Transfer, TransferId> transfers)
    : ICommandHandler<RecordTransferDebit, CommandResult>
{
    public async Task<CommandResult> Handle(RecordTransferDebit command, CancellationToken cancellationToken)
    {
        var transfer = await transfers.Get(new TransferId(command.TransferId), cancellationToken);
        transfer.RecordDebit();
        await transfers.Save(transfer, cancellationToken);
        return new CommandResult(transfer.Id.Value, transfer.Version);
    }
}

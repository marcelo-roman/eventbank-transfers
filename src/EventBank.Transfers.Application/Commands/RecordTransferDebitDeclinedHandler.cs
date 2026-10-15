using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Commands;

public sealed class RecordTransferDebitDeclinedHandler(IEventSourcedRepository<Transfer, TransferId> transfers)
    : ICommandHandler<RecordTransferDebitDeclined, CommandResult>
{
    public async Task<CommandResult> Handle(RecordTransferDebitDeclined command, CancellationToken cancellationToken)
    {
        var transfer = await transfers.Get(new TransferId(command.TransferId), cancellationToken);
        transfer.RecordDebitDeclined(command.Reason);
        await transfers.Save(transfer, cancellationToken);
        return new CommandResult(transfer.Id.Value, transfer.Version);
    }
}

using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Commands;

public sealed class RecordTransferRefundHandler(IEventSourcedRepository<Transfer, TransferId> transfers)
    : ICommandHandler<RecordTransferRefund, CommandResult>
{
    public async Task<CommandResult> Handle(RecordTransferRefund command, CancellationToken cancellationToken)
    {
        var transfer = await transfers.Get(new TransferId(command.TransferId), cancellationToken);
        transfer.RecordRefund();
        await transfers.Save(transfer, cancellationToken);
        return new CommandResult(transfer.Id.Value, transfer.Version);
    }
}

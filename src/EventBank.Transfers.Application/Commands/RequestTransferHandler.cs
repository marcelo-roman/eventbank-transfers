using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Commands;

public sealed class RequestTransferHandler(TransferInitiation initiation) : ICommandHandler<RequestTransfer, CommandResult>
{
    public Task<CommandResult> Handle(RequestTransfer command, CancellationToken cancellationToken) =>
        initiation.Start(command.SourceAccountId, command.DestinationAccountId, command.Amount, cancellationToken);
}

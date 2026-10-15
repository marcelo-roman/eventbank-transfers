using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Messaging;
using EventBank.Contracts;
using EventBank.Transfers.Application.Commands;

namespace EventBank.Transfers.Api.Consumers;

public sealed class AccountRefundedConsumer(ICommandHandler<RecordTransferRefund, CommandResult> handler) : IMessageHandler<AccountRefunded>
{
    public Task Handle(AccountRefunded message, CancellationToken cancellationToken) =>
        handler.Handle(new RecordTransferRefund(message.TransferId), cancellationToken);
}

using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Messaging;
using EventBank.Contracts;
using EventBank.Transfers.Application.Commands;

namespace EventBank.Transfers.Api.Consumers;

public sealed class AccountDebitDeclinedConsumer(ICommandHandler<RecordTransferDebitDeclined, CommandResult> handler) : IMessageHandler<AccountDebitDeclined>
{
    public Task Handle(AccountDebitDeclined message, CancellationToken cancellationToken) =>
        handler.Handle(new RecordTransferDebitDeclined(message.TransferId, message.Reason), cancellationToken);
}

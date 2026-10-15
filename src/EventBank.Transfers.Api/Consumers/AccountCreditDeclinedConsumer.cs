using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Messaging;
using EventBank.Contracts;
using EventBank.Transfers.Application.Commands;

namespace EventBank.Transfers.Api.Consumers;

public sealed class AccountCreditDeclinedConsumer(ICommandHandler<RecordTransferCreditDeclined, CommandResult> handler) : IMessageHandler<AccountCreditDeclined>
{
    public Task Handle(AccountCreditDeclined message, CancellationToken cancellationToken) =>
        handler.Handle(new RecordTransferCreditDeclined(message.TransferId, message.Reason), cancellationToken);
}

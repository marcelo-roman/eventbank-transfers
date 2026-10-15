using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Messaging;
using EventBank.Contracts;
using EventBank.Transfers.Application.Commands;

namespace EventBank.Transfers.Api.Consumers;

public sealed class AccountCreditedConsumer(ICommandHandler<RecordTransferCredit, CommandResult> handler) : IMessageHandler<AccountCredited>
{
    public Task Handle(AccountCredited message, CancellationToken cancellationToken) =>
        handler.Handle(new RecordTransferCredit(message.TransferId), cancellationToken);
}

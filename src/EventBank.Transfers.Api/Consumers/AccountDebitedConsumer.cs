using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Messaging;
using EventBank.Contracts;
using EventBank.Transfers.Application.Commands;

namespace EventBank.Transfers.Api.Consumers;

public sealed class AccountDebitedConsumer(ICommandHandler<RecordTransferDebit, CommandResult> handler) : IMessageHandler<AccountDebited>
{
    public Task Handle(AccountDebited message, CancellationToken cancellationToken) =>
        handler.Handle(new RecordTransferDebit(message.TransferId), cancellationToken);
}

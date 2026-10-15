using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Messaging;
using EventBank.Contracts;
using EventBank.Transfers.Application.Accounts;

namespace EventBank.Transfers.Api.Consumers;

public sealed class AccountClosedConsumer(ICommandHandler<MarkAccountClosed, Unit> handler) : IMessageHandler<AccountClosed>
{
    public Task Handle(AccountClosed message, CancellationToken cancellationToken) =>
        handler.Handle(new MarkAccountClosed(message.AccountId), cancellationToken);
}

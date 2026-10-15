using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Messaging;
using EventBank.Contracts;
using EventBank.Transfers.Application.Accounts;

namespace EventBank.Transfers.Api.Consumers;

public sealed class AccountOpenedConsumer(ICommandHandler<RegisterAccount, Unit> handler) : IMessageHandler<AccountOpened>
{
    public Task Handle(AccountOpened message, CancellationToken cancellationToken) =>
        handler.Handle(new RegisterAccount(message.AccountId, message.Holder, message.Currency), cancellationToken);
}

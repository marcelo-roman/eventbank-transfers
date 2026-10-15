using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Messaging;
using EventBank.Contracts;
using EventBank.Transfers.Application.Customers;

namespace EventBank.Transfers.Api.Consumers;

public sealed class CustomerApprovedConsumer(ICommandHandler<RegisterCustomerProfile, Unit> handler) : IMessageHandler<CustomerApproved>
{
    public Task Handle(CustomerApproved message, CancellationToken cancellationToken) =>
        handler.Handle(new RegisterCustomerProfile(message.CustomerId, message.FullName, message.Email, message.Cpf), cancellationToken);
}

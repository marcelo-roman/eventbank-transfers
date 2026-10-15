using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Customers;

public sealed record RegisterCustomerProfile(Guid CustomerId, string FullName, string Email, string Cpf) : ICommand<Unit>;

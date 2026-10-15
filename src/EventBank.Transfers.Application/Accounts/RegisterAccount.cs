using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Accounts;

public sealed record RegisterAccount(Guid AccountId, Guid OwnerId, string Holder, string Currency, string Number) : ICommand<Unit>;

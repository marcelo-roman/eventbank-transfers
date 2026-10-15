using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Accounts;

public sealed record RegisterAccount(Guid AccountId, string Holder, string Currency) : ICommand<Unit>;

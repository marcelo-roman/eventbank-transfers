using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Accounts;

public sealed record MarkAccountClosed(Guid AccountId) : ICommand<Unit>;

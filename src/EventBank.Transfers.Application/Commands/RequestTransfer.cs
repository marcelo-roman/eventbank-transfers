using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Commands;

public sealed record RequestTransfer(Guid SourceAccountId, Guid DestinationAccountId, decimal Amount)
    : ICommand<CommandResult>;

using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Pix;

public sealed record RegisterPixKey(Guid AccountId, string? Type, string? Value) : ICommand<CommandResult>;

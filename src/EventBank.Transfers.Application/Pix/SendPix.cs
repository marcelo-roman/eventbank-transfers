using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Pix;

public sealed record SendPix(Guid SourceAccountId, string? Key, decimal Amount) : ICommand<CommandResult>;

using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Pix;

public sealed record RemovePixKey(string? Key) : ICommand<CommandResult>;

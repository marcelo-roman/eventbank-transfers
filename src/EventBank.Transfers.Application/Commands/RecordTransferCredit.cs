using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Commands;

public sealed record RecordTransferCredit(Guid TransferId) : ICommand<CommandResult>;

using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Commands;

public sealed record RecordTransferRefund(Guid TransferId) : ICommand<CommandResult>;

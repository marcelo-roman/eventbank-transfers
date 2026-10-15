using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Commands;

public sealed record RecordTransferDebit(Guid TransferId) : ICommand<CommandResult>;

using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Commands;

public sealed record RecordTransferDebitDeclined(Guid TransferId, string Reason) : ICommand<CommandResult>;

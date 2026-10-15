using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Commands;

public sealed record RecordTransferCreditDeclined(Guid TransferId, string Reason) : ICommand<CommandResult>;

namespace EventBank.Transfers.Domain.Events;

public sealed record TransferRequested(
    Guid TransferId,
    Guid SourceAccountId,
    Guid DestinationAccountId,
    decimal Amount,
    string Currency) : ITransferEvent;

public sealed record TransferDebited(Guid TransferId, Guid DestinationAccountId, decimal Amount, string Currency) : ITransferEvent;

public sealed record TransferFailed(Guid TransferId, string Reason) : ITransferEvent;

public sealed record TransferCompleted(
    Guid TransferId,
    Guid SourceAccountId,
    Guid DestinationAccountId,
    decimal Amount,
    string Currency) : ITransferEvent;

public sealed record TransferCreditDeclined(Guid TransferId, Guid SourceAccountId, string Reason) : ITransferEvent;

public sealed record TransferReversed(Guid TransferId) : ITransferEvent;

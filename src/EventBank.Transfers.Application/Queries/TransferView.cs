namespace EventBank.Transfers.Application.Queries;

public sealed record TransferView(
    Guid Id,
    Guid SourceAccountId,
    string SourceHolder,
    Guid DestinationAccountId,
    string DestinationHolder,
    decimal Amount,
    string Currency,
    string Status,
    string? FailureReason,
    long Version,
    DateTimeOffset RequestedAt,
    DateTimeOffset UpdatedAt);

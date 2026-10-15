namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class TransferSummary
{
    public Guid Id { get; init; }

    public Guid SourceAccountId { get; init; }

    public required string SourceHolder { get; init; }

    public Guid DestinationAccountId { get; init; }

    public required string DestinationHolder { get; init; }

    public decimal Amount { get; init; }

    public required string Currency { get; init; }

    public required string Status { get; set; }

    public string? FailureReason { get; set; }

    public long Version { get; set; }

    public DateTimeOffset RequestedAt { get; init; }

    public DateTimeOffset UpdatedAt { get; set; }
}

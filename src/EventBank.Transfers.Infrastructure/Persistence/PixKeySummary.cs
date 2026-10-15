namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class PixKeySummary
{
    public required string Key { get; init; }

    public required string Type { get; set; }

    public Guid AccountId { get; set; }

    public Guid OwnerId { get; set; }

    public required string OwnerName { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset RegisteredAt { get; set; }
}

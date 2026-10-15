namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class AccountDirectoryEntry
{
    public Guid AccountId { get; init; }

    public Guid OwnerId { get; init; }

    public required string Holder { get; init; }

    public required string Currency { get; init; }

    public required string Number { get; init; }

    public bool IsOpen { get; set; }
}

namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class AccountDirectoryEntry
{
    public Guid AccountId { get; init; }

    public required string Holder { get; init; }

    public required string Currency { get; init; }

    public bool IsOpen { get; set; }
}

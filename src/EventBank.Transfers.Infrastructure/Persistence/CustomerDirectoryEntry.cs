namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class CustomerDirectoryEntry
{
    public Guid CustomerId { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public required string Cpf { get; init; }
}

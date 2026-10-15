using EventBank.BuildingBlocks.Domain;

namespace EventBank.Transfers.Domain;

public readonly record struct TransferId(Guid Value) : IEntityId
{
    public static TransferId New() => new(Guid.CreateVersion7());
}

using EventBank.BuildingBlocks.Domain;

namespace EventBank.Transfers.Domain.Pix;

public sealed record PixKeyRegistered(
    Guid PixKeyId,
    string Type,
    string Key,
    Guid AccountId,
    Guid OwnerId,
    string OwnerName) : IDomainEvent;

public sealed record PixKeyRemoved(Guid PixKeyId, string Key, Guid OwnerId) : IDomainEvent;

using EventBank.BuildingBlocks.Domain;

namespace EventBank.Transfers.Domain.Pix;

public readonly record struct PixKeyId(Guid Value) : IEntityId
{
    private static readonly Guid Namespace = Guid.Parse("8b1c4f2e-5d6a-4e3b-9f7c-2a1d0e9b8c7f");

    public static PixKeyId For(PixKeyValue key) => new(DeterministicGuid.Create(Namespace, key.Value));
}

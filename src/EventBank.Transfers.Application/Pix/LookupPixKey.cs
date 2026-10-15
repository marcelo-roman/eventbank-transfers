using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Pix;

public sealed record LookupPixKey(string? Key) : IQuery<PixKeyLookupView>;

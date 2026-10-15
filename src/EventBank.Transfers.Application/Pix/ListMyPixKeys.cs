using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Pix;

public sealed record ListMyPixKeys : IQuery<IReadOnlyList<PixKeyView>>;

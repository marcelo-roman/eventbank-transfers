using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Queries;

public sealed record ListTransfers : IQuery<IReadOnlyList<TransferView>>;

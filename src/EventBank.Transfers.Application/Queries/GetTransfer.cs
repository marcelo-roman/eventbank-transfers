using EventBank.BuildingBlocks.Application.Cqrs;

namespace EventBank.Transfers.Application.Queries;

public sealed record GetTransfer(Guid TransferId) : IQuery<TransferView?>;

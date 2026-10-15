namespace EventBank.Transfers.Api.Endpoints;

public sealed record RequestTransferRequest(Guid SourceAccountId, Guid DestinationAccountId, decimal Amount);

public sealed record RegisterPixKeyRequest(Guid AccountId, string? Type, string? Value);

public sealed record SendPixRequest(Guid SourceAccountId, string? Key, decimal Amount);

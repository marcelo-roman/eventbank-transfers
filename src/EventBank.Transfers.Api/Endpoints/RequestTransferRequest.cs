namespace EventBank.Transfers.Api.Endpoints;

public sealed record RequestTransferRequest(Guid SourceAccountId, Guid DestinationAccountId, decimal Amount);

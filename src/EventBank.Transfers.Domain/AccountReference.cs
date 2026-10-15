namespace EventBank.Transfers.Domain;

public sealed record AccountReference(AccountId Id, Guid OwnerId, Currency Currency, bool IsOpen);

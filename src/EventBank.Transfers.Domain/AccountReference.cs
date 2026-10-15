namespace EventBank.Transfers.Domain;

public sealed record AccountReference(AccountId Id, Currency Currency, bool IsOpen);

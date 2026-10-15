using EventBank.BuildingBlocks.Domain;

namespace EventBank.Transfers.Domain;

public static class TransferErrors
{
    public static DomainException SameAccount() =>
        new("same_account", "Source and destination must be different accounts.");

    public static DomainException AccountClosed(AccountId accountId) =>
        new("account_closed", $"Account '{accountId.Value}' is closed.");

    public static DomainException CurrencyMismatch() =>
        new("currency_mismatch", "Both accounts must operate in the transfer currency.");

    public static DomainException InvalidAmount() =>
        new("invalid_amount", "Amount must be greater than zero and have at most two decimal places.");

    public static DomainException UnsupportedCurrency(string? code) =>
        new("unsupported_currency", $"Currency '{code}' is not supported.");
}

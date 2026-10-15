namespace EventBank.Transfers.Domain;

public sealed record Money
{
    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public Currency Currency { get; }

    public static Money Of(decimal amount, Currency currency)
    {
        if (amount <= 0 || decimal.Round(amount, 2) != amount)
        {
            throw TransferErrors.InvalidAmount();
        }

        return new Money(amount, currency);
    }

    internal static Money Restore(decimal amount, Currency currency) => new(amount, currency);

    public override string ToString() => $"{Currency} {Amount:0.00}";
}

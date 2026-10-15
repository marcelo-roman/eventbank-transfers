using EventBank.BuildingBlocks.Domain;
using EventBank.Transfers.Domain.Events;

namespace EventBank.Transfers.Domain.Tests;

internal static class TransferHistory
{
    public static readonly TransferId PaymentId = new(Guid.Parse("0199b000-0000-7000-8000-000000000001"));
    public static readonly AccountId Source = new(Guid.Parse("0199b000-0000-7000-8000-0000000000a1"));
    public static readonly AccountId Destination = new(Guid.Parse("0199b000-0000-7000-8000-0000000000a2"));
    public static readonly Currency Real = Currency.From("BRL");

    public static readonly TransferRequested Requested = new(PaymentId.Value, Source.Value, Destination.Value, 75m, "BRL");
    public static readonly TransferDebited Debited = new(PaymentId.Value, Destination.Value, 75m, "BRL");

    public static readonly Guid Owner = Guid.Parse("0199b000-0000-7000-8000-0000000000c1");

    public static AccountReference OpenAccount(AccountId id, string currency = "BRL") => new(id, Owner, Currency.From(currency), IsOpen: true);

    public static Transfer Given(params IDomainEvent[] history)
    {
        var transfer = (Transfer)Activator.CreateInstance(typeof(Transfer), nonPublic: true)!;
        transfer.LoadFromHistory(history);
        return transfer;
    }
}

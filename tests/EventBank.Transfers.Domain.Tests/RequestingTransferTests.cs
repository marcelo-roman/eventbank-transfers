using EventBank.BuildingBlocks.Domain;
using static EventBank.Transfers.Domain.Tests.TransferHistory;

namespace EventBank.Transfers.Domain.Tests;

public sealed class RequestingTransferTests
{
    [Fact]
    public void Requesting_between_two_open_accounts_raises_TransferRequested()
    {
        var transfer = Transfer.Request(PaymentId, OpenAccount(Source), OpenAccount(Destination), Money.Of(75m, Real));

        transfer.PendingEvents.ShouldBe([Requested]);
        transfer.Status.ShouldBe(TransferStatus.Requested);
    }

    [Fact]
    public void Requesting_to_the_same_account_is_rejected() =>
        Should.Throw<DomainException>(() =>
                Transfer.Request(PaymentId, OpenAccount(Source), OpenAccount(Source), Money.Of(75m, Real)))
            .Code.ShouldBe("same_account");

    [Fact]
    public void Requesting_from_a_closed_account_is_rejected() =>
        Should.Throw<DomainException>(() =>
                Transfer.Request(PaymentId, OpenAccount(Source) with { IsOpen = false }, OpenAccount(Destination), Money.Of(75m, Real)))
            .Code.ShouldBe("account_closed");

    [Fact]
    public void Requesting_between_accounts_in_different_currencies_is_rejected() =>
        Should.Throw<DomainException>(() =>
                Transfer.Request(PaymentId, OpenAccount(Source), OpenAccount(Destination, "USD"), Money.Of(75m, Real)))
            .Code.ShouldBe("currency_mismatch");
}

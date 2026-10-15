using EventBank.Transfers.Domain.Events;
using static EventBank.Transfers.Domain.Tests.TransferHistory;

namespace EventBank.Transfers.Domain.Tests;

public sealed class TransferSagaTests
{
    [Fact]
    public void A_confirmed_debit_asks_for_the_credit()
    {
        var transfer = Given(Requested);

        transfer.RecordDebit();

        transfer.PendingEvents.ShouldBe([Debited]);
        transfer.Status.ShouldBe(TransferStatus.Debited);
    }

    [Fact]
    public void A_confirmed_credit_completes_the_transfer()
    {
        var transfer = Given(Requested, Debited);

        transfer.RecordCredit();

        transfer.PendingEvents.ShouldBe([new TransferCompleted(PaymentId.Value)]);
        transfer.Status.ShouldBe(TransferStatus.Completed);
    }

    [Fact]
    public void A_declined_debit_fails_the_transfer()
    {
        var transfer = Given(Requested);

        transfer.RecordDebitDeclined("insufficient_funds");

        transfer.PendingEvents.ShouldBe([new TransferFailed(PaymentId.Value, "insufficient_funds")]);
        transfer.FailureReason.ShouldBe("insufficient_funds");
    }

    [Fact]
    public void A_declined_credit_starts_the_compensation_on_the_source_account()
    {
        var transfer = Given(Requested, Debited);

        transfer.RecordCreditDeclined("account_closed");

        transfer.PendingEvents.ShouldBe([new TransferCreditDeclined(PaymentId.Value, Source.Value, "account_closed")]);
        transfer.Status.ShouldBe(TransferStatus.Reversing);
    }

    [Fact]
    public void A_refund_finishes_the_compensation()
    {
        var transfer = Given(Requested, Debited, new TransferCreditDeclined(PaymentId.Value, Source.Value, "account_closed"));

        transfer.RecordRefund();

        transfer.PendingEvents.ShouldBe([new TransferReversed(PaymentId.Value)]);
        transfer.Status.ShouldBe(TransferStatus.Reversed);
    }

    [Fact]
    public void Duplicated_replies_are_ignored()
    {
        var transfer = Given(Requested, Debited);

        transfer.RecordDebit();
        transfer.RecordDebitDeclined("insufficient_funds");

        transfer.PendingEvents.ShouldBeEmpty();
    }

    [Fact]
    public void Replies_that_arrive_out_of_order_are_ignored()
    {
        var transfer = Given(Requested);

        transfer.RecordCredit();
        transfer.RecordRefund();

        transfer.PendingEvents.ShouldBeEmpty();
        transfer.Status.ShouldBe(TransferStatus.Requested);
    }
}

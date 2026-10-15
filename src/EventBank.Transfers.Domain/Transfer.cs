using EventBank.BuildingBlocks.Domain;
using EventBank.Transfers.Domain.Events;

namespace EventBank.Transfers.Domain;

public sealed class Transfer : AggregateRoot<TransferId>
{
    private Transfer()
    {
    }

    public AccountId Source { get; private set; }

    public AccountId Destination { get; private set; }

    public Money Amount { get; private set; } = null!;

    public TransferStatus Status { get; private set; }

    public string? FailureReason { get; private set; }

    public static Transfer Request(TransferId id, AccountReference source, AccountReference destination, Money amount)
    {
        if (source.Id == destination.Id)
        {
            throw TransferErrors.SameAccount();
        }

        EnsureOpen(source);
        EnsureOpen(destination);
        if (amount.Currency != source.Currency || amount.Currency != destination.Currency)
        {
            throw TransferErrors.CurrencyMismatch();
        }

        var transfer = new Transfer();
        transfer.Raise(new TransferRequested(id.Value, source.Id.Value, destination.Id.Value, amount.Amount, amount.Currency.Code));
        return transfer;
    }

    public void RecordDebit()
    {
        if (Status != TransferStatus.Requested)
        {
            return;
        }

        Raise(new TransferDebited(Id.Value, Destination.Value, Amount.Amount, Amount.Currency.Code));
    }

    public void RecordDebitDeclined(string reason)
    {
        if (Status != TransferStatus.Requested)
        {
            return;
        }

        Raise(new TransferFailed(Id.Value, reason));
    }

    public void RecordCredit()
    {
        if (Status != TransferStatus.Debited)
        {
            return;
        }

        Raise(new TransferCompleted(Id.Value, Source.Value, Destination.Value, Amount.Amount, Amount.Currency.Code));
    }

    public void RecordCreditDeclined(string reason)
    {
        if (Status != TransferStatus.Debited)
        {
            return;
        }

        Raise(new TransferCreditDeclined(Id.Value, Source.Value, reason));
    }

    public void RecordRefund()
    {
        if (Status != TransferStatus.Reversing)
        {
            return;
        }

        Raise(new TransferReversed(Id.Value));
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case TransferRequested requested:
                Id = new TransferId(requested.TransferId);
                Source = new AccountId(requested.SourceAccountId);
                Destination = new AccountId(requested.DestinationAccountId);
                Amount = Money.Restore(requested.Amount, Currency.Restore(requested.Currency));
                Status = TransferStatus.Requested;
                break;
            case TransferDebited:
                Status = TransferStatus.Debited;
                break;
            case TransferFailed failed:
                Status = TransferStatus.Failed;
                FailureReason = failed.Reason;
                break;
            case TransferCompleted:
                Status = TransferStatus.Completed;
                break;
            case TransferCreditDeclined declined:
                Status = TransferStatus.Reversing;
                FailureReason = declined.Reason;
                break;
            case TransferReversed:
                Status = TransferStatus.Reversed;
                break;
            default:
                throw new InvalidOperationException($"{nameof(Transfer)} cannot apply {domainEvent.GetType().Name}.");
        }
    }

    private static void EnsureOpen(AccountReference account)
    {
        if (!account.IsOpen)
        {
            throw TransferErrors.AccountClosed(account.Id);
        }
    }
}

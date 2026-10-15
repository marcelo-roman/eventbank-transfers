namespace EventBank.Transfers.Domain;

public enum TransferStatus
{
    Requested,
    Debited,
    Completed,
    Failed,
    Reversing,
    Reversed,
}

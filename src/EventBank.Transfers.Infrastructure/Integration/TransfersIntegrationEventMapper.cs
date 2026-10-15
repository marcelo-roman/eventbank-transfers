using EventBank.BuildingBlocks.EventSourcing.Integration;
using EventBank.BuildingBlocks.EventSourcing.Store;
using EventBank.Contracts;
using EventBank.Transfers.Domain.Events;

namespace EventBank.Transfers.Infrastructure.Integration;

public sealed class TransfersIntegrationEventMapper : IIntegrationEventMapper
{
    public object? Map(EventEnvelope envelope) => envelope.Event switch
    {
        TransferRequested requested => new DebitAccount(requested.TransferId, requested.SourceAccountId, requested.Amount, requested.Currency),
        TransferDebited debited => new CreditAccount(debited.TransferId, debited.DestinationAccountId, debited.Amount, debited.Currency),
        TransferCreditDeclined declined => new RefundAccount(declined.TransferId, declined.SourceAccountId),
        Domain.Events.TransferCompleted completed => new Contracts.TransferCompleted(
            completed.TransferId,
            completed.SourceAccountId,
            completed.DestinationAccountId,
            completed.Amount,
            completed.Currency),
        _ => null,
    };
}

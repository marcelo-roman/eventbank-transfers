using EventBank.BuildingBlocks.Domain;

namespace EventBank.Transfers.Domain.Events;

public interface ITransferEvent : IDomainEvent
{
    Guid TransferId { get; }
}

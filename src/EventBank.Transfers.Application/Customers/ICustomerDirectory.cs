using EventBank.Transfers.Domain.Pix;

namespace EventBank.Transfers.Application.Customers;

public interface ICustomerDirectory
{
    Task<PixKeyOwner?> Find(Guid customerId, CancellationToken cancellationToken);

    Task Register(PixKeyOwner customer, CancellationToken cancellationToken);
}

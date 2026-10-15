using EventBank.Transfers.Application.Customers;
using EventBank.Transfers.Domain.Pix;
using Microsoft.EntityFrameworkCore;

namespace EventBank.Transfers.Infrastructure.Persistence;

public sealed class EfCustomerDirectory(TransfersDbContext dbContext) : ICustomerDirectory
{
    public Task<PixKeyOwner?> Find(Guid customerId, CancellationToken cancellationToken) =>
        dbContext.CustomerDirectory
            .AsNoTracking()
            .Where(customer => customer.CustomerId == customerId)
            .Select(customer => new PixKeyOwner(customer.CustomerId, customer.FullName, customer.Cpf, customer.Email))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task Register(PixKeyOwner customer, CancellationToken cancellationToken)
    {
        if (await dbContext.CustomerDirectory.AnyAsync(entry => entry.CustomerId == customer.CustomerId, cancellationToken))
        {
            return;
        }

        dbContext.CustomerDirectory.Add(new CustomerDirectoryEntry
        {
            CustomerId = customer.CustomerId,
            FullName = customer.FullName,
            Email = customer.Email,
            Cpf = customer.Cpf,
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

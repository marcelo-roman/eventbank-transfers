using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.Transfers.Domain.Pix;

namespace EventBank.Transfers.Application.Customers;

public sealed class RegisterCustomerProfileHandler(ICustomerDirectory directory) : ICommandHandler<RegisterCustomerProfile, Unit>
{
    public async Task<Unit> Handle(RegisterCustomerProfile command, CancellationToken cancellationToken)
    {
        await directory.Register(
            new PixKeyOwner(command.CustomerId, command.FullName, command.Cpf, command.Email.ToLowerInvariant()),
            cancellationToken);
        return Unit.Value;
    }
}

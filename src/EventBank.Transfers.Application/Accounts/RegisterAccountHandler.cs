using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Accounts;

public sealed class RegisterAccountHandler(IAccountDirectory directory) : ICommandHandler<RegisterAccount, Unit>
{
    public async Task<Unit> Handle(RegisterAccount command, CancellationToken cancellationToken)
    {
        await directory.Register(
            new AccountId(command.AccountId),
            command.OwnerId,
            command.Holder,
            command.Currency,
            command.Number,
            cancellationToken);
        return Unit.Value;
    }
}

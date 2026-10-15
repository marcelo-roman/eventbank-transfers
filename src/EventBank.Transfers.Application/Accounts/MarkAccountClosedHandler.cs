using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.Transfers.Domain;

namespace EventBank.Transfers.Application.Accounts;

public sealed class MarkAccountClosedHandler(IAccountDirectory directory) : ICommandHandler<MarkAccountClosed, Unit>
{
    public async Task<Unit> Handle(MarkAccountClosed command, CancellationToken cancellationToken)
    {
        await directory.MarkClosed(new AccountId(command.AccountId), cancellationToken);
        return Unit.Value;
    }
}

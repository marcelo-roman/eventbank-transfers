using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.Transfers.Application.Commands;
using EventBank.Transfers.Domain.Pix;

namespace EventBank.Transfers.Application.Pix;

public sealed class SendPixHandler(IPixKeyReadStore readStore, TransferInitiation initiation)
    : ICommandHandler<SendPix, CommandResult>
{
    public async Task<CommandResult> Handle(SendPix command, CancellationToken cancellationToken)
    {
        var key = PixKeyValue.Detect(command.Key);
        var destination = await readStore.FindActive(key.Value, cancellationToken) ?? throw PixErrors.KeyNotRegistered();
        return await initiation.Start(command.SourceAccountId, destination.AccountId, command.Amount, cancellationToken);
    }
}

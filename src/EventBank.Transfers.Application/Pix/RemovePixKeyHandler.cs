using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.BuildingBlocks.Application.Security;
using EventBank.Transfers.Domain.Pix;

namespace EventBank.Transfers.Application.Pix;

public sealed class RemovePixKeyHandler(IEventSourcedRepository<PixKey, PixKeyId> keys, ICurrentUser currentUser)
    : ICommandHandler<RemovePixKey, CommandResult>
{
    public async Task<CommandResult> Handle(RemovePixKey command, CancellationToken cancellationToken)
    {
        var pixKey = await keys.Find(PixKeyId.For(PixKeyValue.Detect(command.Key)), cancellationToken)
            ?? throw PixErrors.KeyNotRegistered();
        pixKey.Remove(currentUser.Id);
        await keys.Save(pixKey, cancellationToken);
        return new CommandResult(pixKey.Id.Value, pixKey.Version);
    }
}

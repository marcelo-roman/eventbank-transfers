using EventBank.BuildingBlocks.Application.Cqrs;
using EventBank.BuildingBlocks.Application.Persistence;
using EventBank.BuildingBlocks.Application.Security;
using EventBank.Transfers.Application.Accounts;
using EventBank.Transfers.Application.Customers;
using EventBank.Transfers.Domain;
using EventBank.Transfers.Domain.Pix;

namespace EventBank.Transfers.Application.Pix;

public sealed class RegisterPixKeyHandler(
    IEventSourcedRepository<PixKey, PixKeyId> keys,
    IPixKeyReadStore readStore,
    IAccountDirectory accounts,
    ICustomerDirectory customers,
    ICurrentUser currentUser) : ICommandHandler<RegisterPixKey, CommandResult>
{
    public async Task<CommandResult> Handle(RegisterPixKey command, CancellationToken cancellationToken)
    {
        var key = ParseKey(command.Type, command.Value);
        var owner = await customers.Find(currentUser.Id, cancellationToken)
            ?? throw new ForbiddenException("PIX keys are available to approved customers only.");
        var account = await accounts.Find(new AccountId(command.AccountId), cancellationToken)
            ?? throw new NotFoundException("Account", command.AccountId);
        var keysOnAccount = await readStore.CountActiveOnAccount(command.AccountId, cancellationToken);

        var pixKey = await keys.Find(PixKeyId.For(key), cancellationToken);
        if (pixKey is null)
        {
            pixKey = PixKey.Register(key, account, owner, keysOnAccount);
        }
        else
        {
            pixKey.Reclaim(account, owner, keysOnAccount);
        }

        await keys.Save(pixKey, cancellationToken);
        return new CommandResult(pixKey.Id.Value, pixKey.Version);
    }

    private static PixKeyValue ParseKey(string? type, string? value)
    {
        if (!Enum.TryParse<PixKeyType>(type, ignoreCase: true, out var keyType))
        {
            throw PixErrors.UnknownType(type);
        }

        return keyType == PixKeyType.Random && string.IsNullOrWhiteSpace(value)
            ? PixKeyValue.NewRandom()
            : PixKeyValue.Parse(keyType, value);
    }
}

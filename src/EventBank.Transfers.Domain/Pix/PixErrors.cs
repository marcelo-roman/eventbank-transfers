using EventBank.BuildingBlocks.Domain;

namespace EventBank.Transfers.Domain.Pix;

public static class PixErrors
{
    public static DomainException InvalidKey(PixKeyType type) =>
        new("invalid_pix_key", $"This is not a valid {type} key.");

    public static DomainException UnknownType(string? type) =>
        new("invalid_pix_key_type", $"Key type '{type}' does not exist. Use Cpf, Email, Phone or Random.");

    public static DomainException KeyInUse() =>
        new("pix_key_in_use", "This key is already registered.");

    public static DomainException KeyNotYours() =>
        new("pix_key_not_yours", "CPF and e-mail keys must be the account holder's own.");

    public static DomainException AccountNotYours() =>
        new("account_not_yours", "Keys can only point to your own accounts.");

    public static DomainException AccountClosed() =>
        new("account_closed", "Keys cannot point to a closed account.");

    public static DomainException TooManyKeys(int limit) =>
        new("pix_key_limit", $"An account can have at most {limit} keys.");

    public static DomainException KeyNotRegistered() =>
        new("pix_key_not_registered", "This key is not registered.");
}

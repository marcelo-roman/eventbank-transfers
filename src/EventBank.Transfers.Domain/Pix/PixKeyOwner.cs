namespace EventBank.Transfers.Domain.Pix;

public sealed record PixKeyOwner(Guid CustomerId, string FullName, string Cpf, string Email);

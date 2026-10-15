namespace EventBank.Transfers.Application.Pix;

public sealed record PixKeyView(string Key, string Type, string MaskedKey, Guid AccountId, DateTimeOffset RegisteredAt);

public sealed record PixKeyLookupView(string MaskedKey, string Type, string OwnerName, string Institution);

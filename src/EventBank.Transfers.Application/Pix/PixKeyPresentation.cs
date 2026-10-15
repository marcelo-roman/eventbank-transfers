using EventBank.Transfers.Domain.Pix;

namespace EventBank.Transfers.Application.Pix;

internal static class PixKeyPresentation
{
    public const string Institution = "EventBank";

    public static string Mask(ActivePixKey key) =>
        PixKeyValue.Parse(Enum.Parse<PixKeyType>(key.Type), key.Key).Masked();

    public static string ShortName(string fullName)
    {
        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length < 2 ? fullName : $"{parts[0]} {parts[^1][0]}.";
    }
}

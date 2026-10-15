using System.Globalization;

namespace EventBank.Transfers.Domain.Pix;

public sealed record PixKeyValue
{
    private PixKeyValue(PixKeyType type, string value)
    {
        Type = type;
        Value = value;
    }

    public PixKeyType Type { get; }

    public string Value { get; }

    public static PixKeyValue Parse(PixKeyType type, string? raw) => type switch
    {
        PixKeyType.Cpf => new PixKeyValue(type, ParseCpf(raw)),
        PixKeyType.Email => new PixKeyValue(type, ParseEmail(raw)),
        PixKeyType.Phone => new PixKeyValue(type, ParsePhone(raw)),
        PixKeyType.Random => new PixKeyValue(type, ParseRandom(raw)),
        _ => throw PixErrors.UnknownType(type.ToString()),
    };

    public static PixKeyValue NewRandom() => new(PixKeyType.Random, Guid.NewGuid().ToString());

    public static PixKeyValue Detect(string? raw)
    {
        var trimmed = raw?.Trim() ?? string.Empty;
        if (trimmed.Contains('@', StringComparison.Ordinal))
        {
            return Parse(PixKeyType.Email, trimmed);
        }

        if (Guid.TryParse(trimmed, out _))
        {
            return Parse(PixKeyType.Random, trimmed);
        }

        if (trimmed.StartsWith('+'))
        {
            return Parse(PixKeyType.Phone, trimmed);
        }

        return Parse(PixKeyType.Cpf, trimmed);
    }

    internal static PixKeyValue Restore(PixKeyType type, string value) => new(type, value);

    public string Masked() => Type switch
    {
        PixKeyType.Cpf => $"***.{Value[3..6]}.{Value[6..9]}-**",
        PixKeyType.Email => $"{Value[0]}***{Value[Value.IndexOf('@', StringComparison.Ordinal)..]}",
        PixKeyType.Phone => $"{Value[..5]} *****-{Value[^4..]}",
        _ => Value,
    };

    public override string ToString() => Value;

    private static string ParseCpf(string? raw)
    {
        var digits = Digits(raw);
        if (digits.Length != 11 || digits.Distinct().Count() == 1 || !CpfCheckDigitsMatch(digits))
        {
            throw PixErrors.InvalidKey(PixKeyType.Cpf);
        }

        return digits;
    }

    private static string ParseEmail(string? raw)
    {
        var email = raw?.Trim().ToLowerInvariant() ?? string.Empty;
        var at = email.IndexOf('@', StringComparison.Ordinal);
        if (at < 1 || email.LastIndexOf('.') < at + 2 || email.EndsWith('.') || email.Length > 77)
        {
            throw PixErrors.InvalidKey(PixKeyType.Email);
        }

        return email;
    }

    private static string ParsePhone(string? raw)
    {
        var digits = Digits(raw);
        if (digits.StartsWith("55", StringComparison.Ordinal) && digits.Length > 11)
        {
            digits = digits[2..];
        }

        if (digits.Length is < 10 or > 11)
        {
            throw PixErrors.InvalidKey(PixKeyType.Phone);
        }

        return $"+55{digits}";
    }

    private static string ParseRandom(string? raw)
    {
        if (!Guid.TryParse(raw?.Trim(), out var key))
        {
            throw PixErrors.InvalidKey(PixKeyType.Random);
        }

        return key.ToString();
    }

    private static string Digits(string? raw) => new((raw ?? string.Empty).Where(char.IsAsciiDigit).ToArray());

    private static bool CpfCheckDigitsMatch(string digits) =>
        CpfCheckDigit(digits, 9) == digits[9] - '0' && CpfCheckDigit(digits, 10) == digits[10] - '0';

    private static int CpfCheckDigit(string digits, int count)
    {
        var sum = 0;
        for (var index = 0; index < count; index++)
        {
            sum += int.Parse(digits[index].ToString(), CultureInfo.InvariantCulture) * (count + 1 - index);
        }

        var remainder = sum * 10 % 11;
        return remainder == 10 ? 0 : remainder;
    }
}

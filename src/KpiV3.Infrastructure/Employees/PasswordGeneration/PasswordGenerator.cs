using KpiV3.Domain.Employees.Ports;
using Microsoft.Extensions.Options;

namespace KpiV3.Infrastructure.Employees.PasswordGeneration;

public class PasswordGenerator : IPasswordGenerator
{
    private readonly PasswordGeneratorOptions _options;

    public PasswordGenerator(IOptions<PasswordGeneratorOptions> options)
    {
        _options = options.Value;
    }

    private bool AllocateOnStack => _options.Length <= 64;

    public string GeneratePassword()
    {
        Span<char> password = AllocateOnStack ? stackalloc char[_options.Length] : new char[_options.Length];

        for (int i = 0; i < _options.Length; i++)
        {
            if (_options.IncludeUppercase && HitChance(20))
            {
                password[i] = PickUppercaseLetter();
                continue;
            }

            if (_options.IncludeDigits && HitChance(20))
            {
                password[i] = PickDigit();
                continue;
            }

            if (_options.IncludeSymbols && HitChance(10))
            {
                password[i] = PickSymbol();
                continue;
            }

            password[i] = PickLowercaseLetter();
        }

        return password.ToString();
    }

    private static char PickLowercaseLetter()
    {
        return (char)('a' + Random.Shared.Next(26));
    }

    private static char PickUppercaseLetter()
    {
        return (char)('A' + Random.Shared.Next(26));
    }

    private static char PickDigit()
    {
        return (char)('0' + Random.Shared.Next(10));
    }

    private static char PickSymbol()
    {
        const string symbols = "!@#$%^&*()-+=";

        return symbols[Random.Shared.Next(symbols.Length)];
    }

    private static bool HitChance(int probability)
    {
        return Random.Shared.Next(0, 100) <= probability;
    }
}

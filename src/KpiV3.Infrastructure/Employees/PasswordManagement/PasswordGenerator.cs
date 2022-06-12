using KpiV3.Domain.Employees.Ports;
using System.Runtime.CompilerServices;

namespace KpiV3.Infrastructure.Employees.PasswordManagement;

public class PasswordGenerator : IPasswordGenerator
{
    private const int Length = 10;

    public string Generate()
    {
        Span<char> password = stackalloc char[Length];

        for (int i = 0; i < Length; i++)
        {
            password[i] = Random.Shared.Next(4) switch
            {
                0 => Symbol(),
                1 => Uppercase(),
                2 => Lowercase(),
                _ => Digit(),
            };
        }

        return password.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char Symbol()
    {
        const string symbols = "!@#$%^&*()_+";
        return symbols[Random.Shared.Next(symbols.Length)];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char Uppercase() => (char)('A' + Random.Shared.Next(0, 26));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char Lowercase() => (char)('a' + Random.Shared.Next(0, 26));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char Digit() => (char)('0' + Random.Shared.Next(0, 10));
}

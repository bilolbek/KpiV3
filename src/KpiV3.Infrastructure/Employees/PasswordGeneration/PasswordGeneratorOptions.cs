namespace KpiV3.Infrastructure.Employees.PasswordGeneration;

public class PasswordGeneratorOptions
{
    public int Length { get; set; }
    public bool IncludeUppercase { get; set; }
    public bool IncludeDigits { get; set; }
    public bool IncludeSymbols { get; set; }
}

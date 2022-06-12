using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.Validation;

public class GreaterThanAttribute : ValidationAttribute
{
    public GreaterThanAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var currentPropertyName = validationContext.MemberName ?? validationContext.DisplayName;

        if (value is IComparable comparable)
        {
            var property = validationContext.ObjectType.GetProperty(PropertyName);

            if (property is null)
            {
                throw new InvalidOperationException($"Property '{PropertyName}' not found");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);

            if (comparable.CompareTo(comparisonValue) > 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"'{currentPropertyName}' must be greater than '{PropertyName}'");
        }

        return new ValidationResult($"'{currentPropertyName}' must implement {typeof(IComparable)}");
    }
}

using System.ComponentModel.DataAnnotations;

namespace RinhaBackend2023Q3.Attributes;

public class StringArrayRequiredAttribute : ValidationAttribute
{
    private readonly int _stringLength;

    public StringArrayRequiredAttribute(int stringLength)
    {
        _stringLength = stringLength;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is not string[] array ||
            array.Any(item => item.GetType() != typeof(string) || item.Length > _stringLength))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
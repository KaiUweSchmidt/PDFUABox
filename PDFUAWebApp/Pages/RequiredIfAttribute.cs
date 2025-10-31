using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PDFUABox.WebApp.Pages;

public sealed class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _dependentProperty;
    private readonly object _targetValue;

    public string DependentProperty => _dependentProperty;
    public object TargetValue => _targetValue;

    public RequiredIfAttribute(string dependentProperty, object targetValue)
    {
        _dependentProperty = dependentProperty;
        _targetValue = targetValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dependentProperty = validationContext.ObjectType.GetProperty(_dependentProperty);
        if (dependentProperty == null)
        {
            return new ValidationResult($"Property '{_dependentProperty}' not found.");
        }

        var dependentValue = dependentProperty.GetValue(validationContext.ObjectInstance);
        if (dependentValue != null && dependentValue.Equals(_targetValue))
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
            }
        }

        return ValidationResult.Success!;
    }
}

using System.Globalization;
using System.Windows.Controls;
using WPF_MVVM_TEMPLATE.Application.Utility;

namespace WPF_MVVM_TEMPLATE;

public class LettersOnlyRule : ValidationRule
{
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        if (ValidationRegex.ValidateLettersOnly(value.ToString()))
        {
            return new ValidationResult(true, null);
        }
        return new ValidationResult(false, "May only contain letters.");
    }
}
using System.Globalization;
using System.Windows.Controls;
using WPF_MVVM_TEMPLATE.Application.Utility;

namespace WPF_MVVM_TEMPLATE;

public class LettersAndSpacesRule : ValidationRule
{
    
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        if (ValidationRegex.ValidateLettersAndSpaces(value.ToString()))
        {
            return new ValidationResult(true, null);
        }
        return new ValidationResult(false, "Only letters and spaces are allowed");
    }
}
using System.Globalization;
using System.Windows.Controls;
using WPF_MVVM_TEMPLATE.Application.Utility;
using WPF_MVVM_TEMPLATE.Presentation.ViewModel;

namespace WPF_MVVM_TEMPLATE;

public class EmailRule : ValidationRule
{
    
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        if (ValidationRegex.ValidateEmail(value.ToString()))
        {
            return new ValidationResult(true, null);
        }
        return new ValidationResult(false, "Email is invalid");
    }
    
}
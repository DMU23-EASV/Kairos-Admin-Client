using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using WPF_MVVM_TEMPLATE.Application.Utility;

namespace WPF_MVVM_TEMPLATE;

/// <summary>
/// DO NOT IMPLEMENT BEFORE SOME TYPE OF USER NOTIFICATION SYSTEM IS BUILD IN
/// </summary>
public class PasswordRule: ValidationRule
{
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        string? password = value as string;
        const int minimumLength = 8;

        // Check if the Password is null or empty
        if (string.IsNullOrEmpty(password))
        {
            return new ValidationResult(false, "Password cannot be empty.");
        }

        // Validate minimum length
        if (password.Length < minimumLength)
        {
            return new ValidationResult(false, $"Password must be at least {minimumLength} characters long.");
        }

        // Validate at least one uppercase letter
        if (!password.Any(char.IsUpper))
        {
            return new ValidationResult(false, "Password must contain at least one uppercase letter.");
        }

        // Validate at least one lowercase letter
        if (!password.Any(char.IsLower))
        {
            return new ValidationResult(false, "Password must contain at least one lowercase letter.");
        }

        // Validate at least one digit
        if (!password.Any(char.IsDigit))
        {
            return new ValidationResult(false, "Password must contain at least one digit.");
        }

        // Validate at least one special character
        if (ValidationRegex.ValidateAnySpecialCharacter(password))
        {
            return new ValidationResult(false, "Password must contain at least one special character.");
        }

        // If all rules are satisfied
        return new ValidationResult(true, null);
    }
}
using System.Text.RegularExpressions;

namespace WPF_MVVM_TEMPLATE.Application.Utility;

public static class ValidationRegex
{
    /// <summary>
    /// Returns TRUE if only ONE NUMBER, otherwise FALSE
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>
    public static bool ValidateNumber(string value)
    {
        //Regex check for number only (0-9) 
        string pattern = @"^\d$";
        if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern))
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Returns TRUE if only NUMBERS, otherwise FALSE
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>
    public static bool ValidateNumbers(string value)
    {
        //Regex check for numbers only (0-9) 
        string pattern = @"^\d+$";
        if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Returns TRUE if email is valid otherwise FALSE 
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>
    public static bool ValidateEmail(string value)
    {
        //Regex check for 1 (@) and 1 OR more .
        string pattern = @"^[^@]+@[^@]+\.[^@]+$";
        if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Returns TRUE if only letters detected otherwise FALSE
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>
    public static bool ValidateLettersOnly(string value)
    {
        // Regex to allow only letters (A-Z, a-z) and spaces
        string pattern = @"^[a-zA-Z]+$";
        if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern))
        {
            return false; 
        }
        return true; 
    }
    
    /// <summary>
    /// Returns TRUE if only letters and spaces are detected, otherwise FALSE
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>
    public static bool ValidateLettersAndSpaces(string value)
    {
        // Regex to allow only letters (A-Z, a-z) and spaces
        string pattern = @"^[a-zA-Z\s]+$"; // \s allows spaces
        if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern))
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Returns TRUE if there is ANY special character in the string
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>
    public static bool ValidateAnySpecialCharacter(string value)
    {
        // Regex to allow only letters (A-Z, a-z) and spaces
        string pattern = @"[\W_]"; // \s allows spaces
        if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern))
        {
            return false;
        }
        return true;
    }
}
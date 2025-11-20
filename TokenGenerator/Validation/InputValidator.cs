using System.Text.RegularExpressions;
using Formatter.Configuration;

namespace TokenGenerator.Validation;

/// <summary>
/// Provides utility methods for validating input string formats
/// </summary>
public static class InputValidator
{
    public static bool ValidateListFormat(this string text)
    {
        const string pattern = @"\bList\s*<[^<>]+>";
        return Regex.IsMatch(text, pattern,RegexOptions.IgnoreCase);
    }
    
    public static bool ValidateArrayFormat(this string text)
    {
        const string pattern = @"\b\w+\s*\[\s*(?:,\s*)*\]";
        return Regex.IsMatch(text, pattern,RegexOptions.IgnoreCase);
    }

    public static bool ValidateDictionaryFormat(this string text)
    {
        const string pattern = @"\bDictionary\s*<[^<>]+>";
        return Regex.IsMatch(text, pattern,RegexOptions.IgnoreCase);
    }

    public static bool ValidateNullableFormat(this string text)
    {
        const string pattern = @"^[A-Za-z]+\s*(?:<[^<>]+>)?\s*(?:\[\s*(?:,\s*)*\])?\?$";
        return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
    }

    public static bool ValidateDateFormat(this string text)
    {
        return text.Replace("?","") == "Date";
    }
    
}
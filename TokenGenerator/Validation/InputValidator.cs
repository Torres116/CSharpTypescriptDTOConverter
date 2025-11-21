using System.Text.RegularExpressions;

namespace TokenGenerator.Validation;

/// <summary>
/// Provides utility methods for validating input string formats
/// </summary>
public static partial class InputValidator
{
    public static bool ValidateListFormat(this string text)
    {
        const string pattern = @"\bList\s*<[^<>]+>";
        return ListRegex().IsMatch(text);
    }

    public static bool ValidateArrayFormat(this string text)
    {
        const string pattern = @"\b\w+\s*\[\s*(?:,\s*)*\]";
        return ArrayRegex().IsMatch(text);
    }

    public static bool ValidateDictionaryFormat(this string? text)
    {
        const string pattern = @"\bDictionary\s*<\s*[^<>]*\s*>";
        return DictionaryRegex().IsMatch(text ?? string.Empty);
    }

    public static bool ValidateNullableFormat(this string text)
    {
        const string pattern = @"^[A-Za-z]+\s*(?:<[^<>]+>)?\s*(?:\[\s*(?:,\s*)*\])?\?$";
        return NullableRegex().IsMatch(text);
    }

    public static bool ValidateDateFormat(this string text)
    {
        return text.Replace("?", "") == "Date";
    }

    [GeneratedRegex(@"\bList\s*<[^<>]+>", RegexOptions.IgnoreCase, "pt-PT")]
    private static partial Regex ListRegex();

    [GeneratedRegex(@"\b\w+\s*\[\s*(?:,\s*)*\]", RegexOptions.IgnoreCase, "pt-PT")]
    private static partial Regex ArrayRegex();

    [GeneratedRegex(@"\bDictionary\s*<\s*[^<>]*\s*>", RegexOptions.IgnoreCase, "pt-PT")]
    private static partial Regex DictionaryRegex();

    [GeneratedRegex(@"^[A-Za-z]+\s*(?:<[^<>]+>)?\s*(?:\[\s*(?:,\s*)*\])?\?$", RegexOptions.IgnoreCase, "pt-PT")]
    private static partial Regex NullableRegex();
}
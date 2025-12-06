using System.Text.RegularExpressions;

namespace TokenGenerator.Validation;

/// <summary>
/// Provides utility methods for validating input string formats
/// </summary>
public static partial class InputValidator
{
    public static bool ValidateListFormat(this string text)
    {
        return ListRegex().IsMatch(text);
    }

    public static bool ValidateArrayFormat(this string text)
    {
        return ArrayRegex().IsMatch(text);
    }

    public static bool ValidateDictionaryFormat(this string? text)
    {
        return DictionaryRegex().IsMatch(text ?? string.Empty);
    }

    public static int GetDictionaryCount(this string? text)
    {
        return DictionaryRegex().Matches(text ?? string.Empty).Count;
    }

    public static bool ValidateNullableFormat(this string text)
    {
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
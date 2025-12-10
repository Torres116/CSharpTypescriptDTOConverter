using System.Text.RegularExpressions;

namespace TokenGenerator.Validation;

/// <summary>
/// Provides utility methods for validating token string formats
/// </summary>
public static partial class TokenValidator
{
    public static bool ValidateListFormat(this string text)
    {
        return ListRegex().IsMatch(text.Trim());
    }

    public static bool ValidateArrayFormat(this string text)
    {
        return ArrayRegex().IsMatch(text.Trim());
    }

    public static bool ValidateDictionaryFormat(this string text)
    {
        return DictionaryRegex().IsMatch(text.Trim());
    }

    public static bool IsNestedDictionary(this string text)
    {
        return NestedDictionaryRegex().IsMatch(text.Trim());
    }

    public static bool ValidateNullableFormat(this string text)
    {
        return NullableRegex().IsMatch(text.Trim());
    }

    public static bool ValidateDateFormat(this string text) => text == "Date";

    [GeneratedRegex(@"^List<.+>$", RegexOptions.Compiled)]
    private static partial Regex ListRegex();

    [GeneratedRegex(@"^.*\[\]$", RegexOptions.Compiled)]
    private static partial Regex ArrayRegex();

    [GeneratedRegex(@"^Dictionary<.*>$", RegexOptions.Compiled)]
    private static partial Regex DictionaryRegex();

    [GeneratedRegex(@"^Dictionary<.*Dictionary<.*>.*>$", RegexOptions.Compiled)]
    private static partial Regex NestedDictionaryRegex();

    [GeneratedRegex(@".*\?$", RegexOptions.Compiled)]
    private static partial Regex NullableRegex();
}
namespace TokenGenerator.utils;

public static class TokenUtils
{
    public static string RemoveDictionary(this string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return string.Empty;

        type = type.Trim();
        const string prefix = "Dictionary<";
        const string suffix = ">";

        if (type.StartsWith(prefix, StringComparison.Ordinal) && type.EndsWith(suffix, StringComparison.Ordinal))
        {
            var temp = type.Substring(prefix.Length, type.Length - prefix.Length - 1);
            return temp;
        }

        return type;
    }

    public static string RemoveGenerics(this string type)
    {
        if (string.IsNullOrWhiteSpace(type) || !type.Contains('<'))
            return type;

        var index = type.IndexOf('<');
        type = type.Substring(0, index);
        
        return type;
    }

    public static string RemoveListAndArray(this string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return string.Empty;
        
        if(type.EndsWith("[]", StringComparison.Ordinal))
            return RemoveListAndArray(type.Substring(0, type.Length - 2));

        const string prefix = "List<";
        const string suffix = ">";

        if (type.StartsWith(prefix, StringComparison.Ordinal) && type.EndsWith(suffix, StringComparison.Ordinal))
            return type.Substring(prefix.Length, type.Length - prefix.Length - suffix.Length);

        return type;
    }
}
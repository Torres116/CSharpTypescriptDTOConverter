namespace Parser;

public static class Keywords
{
    public static readonly HashSet<string> IgnoredKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "public",
        "protected",
        "internal",
        "async",
        "enum"
    };

    public static readonly HashSet<string> DeclarationKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "class",
        "record",
        "struct"
    };

    public static readonly HashSet<string> Types = new(StringComparer.OrdinalIgnoreCase)
    {
        "class",
        "interface",
        "record",
        "string",
        "char",
        "bool",
        "int",
        "float",
        "double",
        "byte",
        "guid",
        "decimal",
        "datetime",
        "timespan",
        
    };
}
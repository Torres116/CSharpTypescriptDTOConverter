using Formatter.Configuration;
using TokenGenerator.interfaces;

namespace TokenGenerator.Handlers;

internal static class ParsedTokenNormalizer
{
    public static void NormalizeIdentifier(IParsedToken token)
    {
        if (!FormatConfiguration.IncludeOptionals)
            token.Identifier = token.Identifier.Replace("?", "");
    }

    public static void NormalizeType(IParsedToken token)
    {
        if (token.IsArray)
            token.Type = $"{token.Type}[]";

        if (token.IsNull && FormatConfiguration.IncludeNullables)
            token.Type = $"{token.Type} | null";
    }
}
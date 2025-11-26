using System.Text.RegularExpressions;
using TokenGenerator.interfaces;
using TokenGenerator.utils;
using TokenGenerator.Validation;

namespace TokenGenerator.Handlers.Type;

internal sealed partial class DictionaryConversionHandler(ITokenGenerator generator) : ITokenHandler
{
    public void Verify(IParsedToken token)
    {
        var result = token.Type.ValidateDictionaryFormat() || token.IsDictionary;
        token.IsDictionary = result;
    }

    public void Convert(IParsedToken token)
    {
        if (!token.IsDictionary || token.Type == null)
            return;

        string? type1;
        string? type2;

        var isMap = false;

        var dictionaryCount = DictionaryRegex().Matches(token.Type).Count;
        if (dictionaryCount > 1)
        {
            var depth = 0;
            var pos = 0;
            var tempType = token.Type.Substring(token.Type.IndexOf('<') + 1);
            for (var index = 0; index < tempType.Length - 1; index++)
            {
                var c = tempType[index];

                if (depth == 0 && c == ',')
                {
                    pos = index;
                    break;
                }

                if (depth == 1 && c == '>')
                {
                    pos = index + 1;
                    isMap = true;
                    break;
                }

                if (c == '<')
                {
                    depth++;
                    continue;
                }

                if (c == '>')
                    depth--;
            }

            type1 = tempType[..pos];
            type2 = tempType[++pos..];
        }
        else
        {
            var types = token.Type?.RemoveDictionary().Split(",") ?? [];
            type1 = types[0]?.Trim();
            type2 = types[1]?.Trim();
        }

        // Check if the first type is a nested dictionary since C# allows reference types as keys.
        var hasNestedDictionary = type1.ValidateDictionaryFormat() || isMap;
        type1 = hasNestedDictionary ? type1 : type1.RemoveDictionary();
        var token1 = generator.ConvertType(new TypescriptToken
            { Type = type1, Identifier = "", Skip = true, IsDictionary = hasNestedDictionary });

        hasNestedDictionary = type2.ValidateDictionaryFormat();
        type2 = hasNestedDictionary ? type2 : type2.RemoveDictionary();
        var token2 = generator.ConvertType(new TypescriptToken
            { Type = type2, Identifier = "", Skip = true, IsDictionary = hasNestedDictionary });

        if (token1.CustomTypes != null || token2.CustomTypes != null)
        {
            token.IsCustomType = true;

            if (token1.CustomTypes != null)
            {
                var customTypes = token1.CustomTypes.Concat(token2.CustomTypes ?? []).ToArray();
                token.CustomTypes = customTypes;
            }
            else
                token.CustomTypes = token2.CustomTypes;
        }

        // if the first type is a nested dictionary
        var type = isMap ? "Map" : "Record";
        token.Type = $"{type}<{token1.Type},{token2.Type}>";
    }

    [GeneratedRegex("Dictionary")]
    private static partial Regex DictionaryRegex();
}
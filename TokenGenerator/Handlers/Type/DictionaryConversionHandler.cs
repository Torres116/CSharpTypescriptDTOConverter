using TokenGenerator.interfaces;
using TokenGenerator.utils;
using TokenGenerator.Validation;

namespace TokenGenerator.Handlers.Type;

internal sealed class DictionaryConversionHandler(ITokenGenerator generator) : ITokenHandler
{
    public void Verify(TypescriptToken token)
    {
        var result = token.Type.ValidateDictionaryFormat() || token.IsDictionary;
        token.IsDictionary = result;
    }

    public void Convert(TypescriptToken token)
    {
        if (!token.IsDictionary)
            return;

        string? type1;
        string? type2;
        string?[] types;

        var nestedDictionaryCount = token.Type?.Split("Dictionary").Length;
        if (nestedDictionaryCount > 2)
        {
            types = token.Type?.Split(",", StringSplitOptions.TrimEntries) ?? [];
            type1 = types?[0];
            type2 = string.Join(",", types?.Skip(1) ?? []);
        }
        else
        {
            types = [token.Type?.RemoveDictionary()];
            type1 = types[0]?.Split(",")[0].Trim();
            type2 = types[0]?.Split(",")[1].Trim();
        }

        var hasNestedDictionary = type1.ValidateDictionaryFormat();
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


        token.Type = $"Record<{token1.Type},{token2.Type}>";
    }
}
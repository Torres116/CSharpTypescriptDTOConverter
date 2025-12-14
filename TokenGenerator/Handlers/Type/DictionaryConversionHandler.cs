using TokenGenerator.interfaces;
using TokenGenerator.utils;
using TokenGenerator.Validation;

namespace TokenGenerator.Handlers.Type;

internal sealed class DictionaryConversionHandler(ITokenGenerator generator) : ITokenHandler
{
    public void Verify(IParsedToken token)
    {
        var isDictionaryFormat = token.Type.ValidateDictionaryFormat();
        token.IsDictionary = isDictionaryFormat;
        if (isDictionaryFormat)
            token.Skip.Add(SkipOptions.CustomType);
    }

    public void Convert(IParsedToken token)
    {
        if (!token.IsDictionary || token.Skip.Contains(SkipOptions.Dictionary))
            return;

        string? type1;
        string? type2;
        var isMap = false;

        var isNestedDictionary = token.Type.IsNestedDictionary();
        if (isNestedDictionary)
        {
            var depth = 0;
            var pos = 0;
            var tempType = token.Type.RemoveDictionary();
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
                    depth++;

                else if (c == '>')
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

        var keyToken = generator.ConvertType(new TypescriptToken
            { Type = type1, Identifier = "", Skip = { SkipOptions.Nullable } });

        var valueToken = generator.ConvertType(new TypescriptToken
            { Type = type2, Identifier = "", Skip = { SkipOptions.Nullable } });

        if (keyToken.CustomTypes != null || valueToken.CustomTypes != null)
        {
            token.IsCustomType = true;

            if (keyToken.CustomTypes != null)
            {
                var customTypes = keyToken.CustomTypes.Concat(valueToken.CustomTypes ?? []).ToArray();
                token.CustomTypes = customTypes;
            }
            else
                token.CustomTypes = valueToken.CustomTypes;
        }

        if(!isMap)
            isMap = keyToken.IsDictionary || keyToken.IsArray;
        
        // if the key is a nested dictionary or Array
        var type = isMap ? "Map" : "Record";
        token.Type = $"{type}<{keyToken.Type},{valueToken.Type}>";
    }
}
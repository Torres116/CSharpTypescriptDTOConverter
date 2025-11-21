using TokenGenerator.interfaces;
using TokenGenerator.Validation;

namespace TokenGenerator.Handlers.Type;

internal sealed class DictionaryConversionHandler(ITokenGenerator generator) : ITokenHandler
{
    public void Verify(TypescriptToken token)
    {
        var result = token.Type.ValidateDictionaryFormat();
        token.IsDictionary = result;
    }

    public void Convert(TypescriptToken token)
    {
        if (!token.IsDictionary)
            return;

        var types = token.Type?.Replace("Dictionary", "").Replace("<", "").Replace(">", "");
        var type1 = types?.Split(",")[0].Trim();
        var type2 = types?.Split(",")[1].Trim();

        var temp1 = generator.ConvertType(new TypescriptToken { Type = type1, Identifier = "", Skip = true });
        var temp2 = generator.ConvertType(new TypescriptToken { Type = type2, Identifier = "", Skip = true });

        if (temp1.CustomTypes != null || temp2.CustomTypes != null)
        {
            token.IsCustomType = true;

            if (temp1.CustomTypes != null)
            {
                var customTypes = temp1.CustomTypes.Concat(temp2.CustomTypes ?? []).ToArray();
                token.CustomTypes = customTypes;
            }
            else
                token.CustomTypes = temp2.CustomTypes;
        }

        token.Type = $"Record<{type1},{type2}>";
    }
}
using TokenGenerator.interfaces;
using TokenGenerator.Validation;

namespace TokenGenerator.Handlers.Type;

public class DictionaryConversionHandler(ITokenGenerator generator) : ITokenHandler
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
        
        var types = token.Type.Replace("Dictionary", "").Replace("<", "").Replace(">", "");
        var type1 = types.Split(",")[0].Trim();
        var type2 = types.Split(",")[1].Trim();
        
        type1 = generator.ConvertType(new TypescriptToken { Type = type1, Identifier = "",Skip = true}).Type;
        type2 = generator.ConvertType(new TypescriptToken { Type = type2, Identifier = "",Skip = true}).Type;
        
        token.Type = $"Record<{type1},{type2}>";
    }
}
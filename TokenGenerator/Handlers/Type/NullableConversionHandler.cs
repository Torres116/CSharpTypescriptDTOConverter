using Formatter.Configuration;
using TokenGenerator.interfaces;
using TokenGenerator.Validation;

namespace TokenGenerator.Handlers.Type;

public class NullableConversionHandler(ITokenGenerator generator) : ITokenHandler
{
    public void Verify(TypescriptToken token)
    {
        if (token.Skip)
            return;
        
        var result = token.Type.ValidateNullableFormat();
        token.IsNull = result;
    }

    public void Convert(TypescriptToken token)
    {
        if (!token.IsNull || !FormatConfiguration.IncludeNullables)
            return;
        
        token.Type = token.Type.Replace("?", "");
        token.Type += " | null";
    }
}
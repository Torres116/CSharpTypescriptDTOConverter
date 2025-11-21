using Formatter.Configuration;
using TokenGenerator.interfaces;
using TokenGenerator.Validation;

namespace TokenGenerator.Handlers.Type;

internal sealed class DateAsStringConversionHandler(ITokenGenerator generator) : ITokenHandler
{
    public void Verify(TypescriptToken token)
    {
        var result = token.Type.ValidateDateFormat();
        token.IsDate = result;
    }

    public void Convert(TypescriptToken token)
    {
        if (FormatConfiguration.DatesAsStrings && token.IsDate) 
            token.Type = "string";
    }
}
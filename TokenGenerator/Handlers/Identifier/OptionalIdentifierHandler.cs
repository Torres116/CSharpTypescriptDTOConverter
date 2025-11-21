using Formatter.Configuration;
using TokenGenerator.interfaces;

namespace TokenGenerator.Handlers.Identifier;

internal class OptionalIdentifierHandler(ITokenGenerator generator) : ITokenHandler
{
    public void Verify(TypescriptToken token)
    {
        var result = token.IsNull;
        token.IsOptional = result;
    }

    public void Convert(TypescriptToken token)
    {
        if (!token.IsOptional || !FormatConfiguration.IncludeOptionals)
            return;

        token.Identifier = $"{token.Identifier}?";
    }
}
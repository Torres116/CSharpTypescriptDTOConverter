using TokenGenerator.interfaces;
using TokenGenerator.utils;
using TokenGenerator.Validation;

namespace TokenGenerator.Handlers.Type;

internal sealed class ListConversionHandler(ITokenGenerator generator) : ITokenHandler
{
    public void Verify(IParsedToken token)
    {
        var isListFormat = token.Type.ValidateArrayFormat() || token.Type.ValidateListFormat();
        token.IsArray = isListFormat;
    }

    public void Convert(IParsedToken token)
    {
        if (!token.IsArray || token.Skip.Contains(SkipOptions.List))
            return;

        token.Type = token.Type.RemoveListAndArray();
        token.Type = generator.ConvertType(new TypescriptToken { Type = token.Type }).Type;

        token.Skip.Add(SkipOptions.CustomType);
    }
}
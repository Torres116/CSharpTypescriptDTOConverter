using TokenGenerator.interfaces;
using TokenGenerator.Validation;

namespace TokenGenerator.Handlers.Type;

internal sealed class NullableConversionHandler(ITokenGenerator generator) : ITokenHandler
{
    public void Verify(IParsedToken token)
    {
        var isNullableFormat = token.Type.ValidateNullableFormat();
        token.IsNull = isNullableFormat;
        token.Type = token.Type.Replace("?", "");
    }

    public void Convert(IParsedToken token)
    {
    }
}
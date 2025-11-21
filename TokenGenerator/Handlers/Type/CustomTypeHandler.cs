using TokenGenerator.interfaces;
using TokenGenerator.utils;

namespace TokenGenerator.Handlers.Type;

internal sealed class CustomTypeHandler : ITokenHandler
{
    public void Verify(TypescriptToken token)
    {
    }

    public void Convert(TypescriptToken token)
    {
        if (token.TokenCustomTypeSkip || token.IsDictionary)
            return;

        var type = token.Type?.Replace("?", "").Replace("[]", "");
        type = type?.Split(" ").First();

        var result = PrimitiveTypeMapper.Types.Values.FirstOrDefault(x => x.Contains(type));
        token.IsCustomType = result == null;
        token.TokenCustomTypeSkip = true;

        if (token.IsCustomType)
            token.CustomTypes = [type.RemoveGenerics()];
    }
}
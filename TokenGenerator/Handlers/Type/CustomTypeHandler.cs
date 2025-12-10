using System.Text.RegularExpressions;
using TokenGenerator.interfaces;
using TokenGenerator.utils;
using TokenGenerator.Validation;

namespace TokenGenerator.Handlers.Type;

internal sealed partial class CustomTypeHandler : ITokenHandler
{
    public void Verify(IParsedToken token)
    {
        if (token.Skip.Contains(SkipOptions.CustomType))
            return;

        if (token.Type.ValidateDictionaryFormat())
            return;

        var typeName = GetBaseTypeName(token.Type);
        var isCustomType = !PrimitiveTypeMapper.Types.ContainsKey(typeName.ToLowerInvariant());
        token.IsCustomType = isCustomType;
    }

    public void Convert(IParsedToken token)
    {
        if (token.Skip.Contains(SkipOptions.CustomType) || !token.IsCustomType)
            return;

        var typeName = GetBaseTypeName(token.Type);
        token.CustomTypes = [typeName];
    }

    private static string GetBaseTypeName(string? rawType)
    {
        if (string.IsNullOrEmpty(rawType))
            return string.Empty;

        rawType = rawType.RemoveListAndArray();
        var match = TypeRegex().Match(rawType);
        
        if (match.Success)
            return match.Value.Substring(1, match.Value.Length - 2).Trim();

        var type = rawType;
        var index = type.IndexOf(' ');

        if (index > -1)
            type = type.Substring(index);

        return type;
    }

    [GeneratedRegex(@"<.*>")]
    private static partial Regex TypeRegex();
}
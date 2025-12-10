using Formatter.Configuration;
using TokenGenerator.Handlers;
using TokenGenerator.Handlers.Identifier;
using TokenGenerator.Handlers.Type;
using TokenGenerator.interfaces;

namespace TokenGenerator;

public sealed class TypescriptTokenGenerator : ITokenGenerator
{
    private List<ITokenHandler>? _typeHandlers;
    private List<ITokenHandler>? _identifierHandlers;

    public TypescriptTokenGenerator()
    {
        InitTypeHandlers();
        InitIdentifierHandlers();
    }

    private void InitTypeHandlers()
    {
        _typeHandlers = new()
        {
            new NullableConversionHandler(this),
            new DateAsStringConversionHandler(this),
            new ListConversionHandler(this),
            new CustomTypeHandler(),
            new DictionaryConversionHandler(this),
        };
    }

    private void InitIdentifierHandlers()
    {
        _identifierHandlers = new()
        {
            new OptionalIdentifierHandler(this)
        };
    }

    public IParsedToken ConvertToken(IToken token)
    {
        if (string.IsNullOrWhiteSpace(token.Identifier) || string.IsNullOrWhiteSpace(token.Type))
            return new TypescriptToken();
        
        var result = new TypescriptToken
        {
            Identifier = token.Identifier,
            Type = token.Type,
            IsComment = token.IsComment,
            Comment = token.Comment,
            IsDeclaration = token.IsDeclaration
        };

        switch (token.IsComment)
        {
            case true when FormatConfiguration.IncludeComments: return result;
            case true: return null;
        }

        ConvertType(result);
        ConvertIdentifier(result);
        return result;
    }

    public IParsedToken ConvertType(IParsedToken token)
    {
        if (_typeHandlers == null)
            return token;
        
        PrimitiveTypeMapper.Convert(token);

        foreach (var handler in _typeHandlers)
            handler.Verify(token);

        foreach (var handler in _typeHandlers)
            handler.Convert(token);

        ParsedTokenNormalizer.NormalizeType(token);
        return token;
    }

    public IParsedToken ConvertIdentifier(IParsedToken token)
    {
        if (_identifierHandlers == null)
            return token;
        
        foreach (var handler in _identifierHandlers)
            handler.Verify(token);

        foreach (var handler in _identifierHandlers)
            handler.Convert(token);

        ParsedTokenNormalizer.NormalizeIdentifier(token);
        return token;
    }
}
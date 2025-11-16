using Formatter.Formatter;
using Formatter.Options;
using TokenGenerator.Handlers;
using TokenGenerator.Handlers.Identifier;
using TokenGenerator.Handlers.Type;
using TokenGenerator.interfaces;

namespace TokenGenerator;

public class TypescriptTokenGenerator : ITokenGenerator
{
    private List<ITokenHandler>? _typeHandlers;
    private List<ITokenHandler>? _identifierHandlers;
    private readonly PrimitiveTypeMapper _primitiveMapper;

    public TypescriptTokenGenerator()
    {
        InitTypeHandlers();
        InitIdentifierHandlers();
        _primitiveMapper = new();
    }

    private void InitTypeHandlers()
    {
        _typeHandlers = new()
        {
            new DateAsStringConversionHandler(this),
            new DictionaryConversionHandler(this),
            new ListConversionHandler(this),
            new NullableConversionHandler(this)
        };
    }

    private void InitIdentifierHandlers()
    {
        _identifierHandlers = new()
        {
            new OptionalIdentifierHandler(this)
        };
    }

    public TypescriptToken? InterpretToken(Token token)
    {
        var result = new TypescriptToken
        {
            Identifier = token.Identifier ?? "",
            Type = token.Type ?? "",
            IsComment = token.IsComment,
            Comment = token.Comment
        };
        
        switch (token.IsComment)
        {
            case true when FormatOptions.IncludeComments: return result;
            case true: return null;
        }

        ConvertType(result);
        ConvertIdentifier(result);
        CleanupHandler.Cleanup(result);
        return result;
    }

    public TypescriptToken ConvertType(TypescriptToken token)
    {
        _primitiveMapper.Convert(token);
        
        foreach (var handler in _typeHandlers ?? [])
            handler.Verify(token);

        foreach (var handler in _typeHandlers ?? [])
            handler.Convert(token);

        return token;
    }

    public TypescriptToken ConvertIdentifier(TypescriptToken token)
    {
        token.Identifier = token.Identifier.Trim();
        
        foreach (var handler in _identifierHandlers ?? [])
            handler.Verify(token);

        foreach (var handler in _identifierHandlers ?? [])
            handler.Convert(token);

        return token;
    }
    
}
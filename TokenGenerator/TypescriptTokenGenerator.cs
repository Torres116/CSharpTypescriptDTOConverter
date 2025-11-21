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
            new DateAsStringConversionHandler(this),
            new ListConversionHandler(this),
            new CustomTypeHandler(),
            new DictionaryConversionHandler(this),
            new NullableConversionHandler(this),
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
            Comment = token.Comment,
            IsDeclaration = token.IsDeclaration,
            IsCustomType = token.IsCustomType 
        };
        
        switch (token.IsComment)
        {
            case true when FormatConfiguration.IncludeComments: return result;
            case true: return null;
        }

        ConvertType(result);
        ConvertIdentifier(result);
        CleanupHandler.Cleanup(result);
        return result;
    }

    public TypescriptToken ConvertType(TypescriptToken token)
    {
        PrimitiveTypeMapper.Convert(token);
        
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
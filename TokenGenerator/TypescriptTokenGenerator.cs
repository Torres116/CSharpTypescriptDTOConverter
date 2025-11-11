using TokenGenerator.handlers;
using TokenGenerator.interfaces;

namespace TokenGenerator;

public class TypescriptTokenGenerator : ITokenGenerator
{
    private List<ITokenTypeHandler>? _typeHandlers;

    public TypescriptTokenGenerator()
    {
        InitTypeHandlers();
    }

    private void InitTypeHandlers()
    {
        _typeHandlers = new List<ITokenTypeHandler>
        {
            new PrimitiveConversionHandler(),
            new DictionaryTypeConversionHandler(this),
            new ListTypeConversionHandler(this),
            new NullableTypeConversionHandler(this)
        };
        
    }

    public TypescriptToken InterpretToken(Token token)
    {
        var convertedType = Convert(token.Type ?? string.Empty);
        var t = new TypescriptToken
        {
            Type = convertedType,
            Identifier = token.Identifier
        };

        return t;
    }

    public string Convert(string type)
    {
        foreach (var handler in _typeHandlers ?? [])
        {
            if (handler.CanConvert(type))
                type = handler.Convert(type);
        }

        return type;
    }
}
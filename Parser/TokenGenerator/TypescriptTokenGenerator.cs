using Parser.Context;
using Parser.Interfaces;
using Parser.Token;
using Parser.TokenGenerator.strategies;

namespace Parser.TokenGenerator;

public class TypescriptTokenGenerator : ITokenGenerator
{
    private List<ITokenTypeHandler> _typeHandlers;

    public TypescriptToken InterpretToken(Token.Token token)
    {

        _typeHandlers = new List<ITokenTypeHandler>
        {
            new PrimitiveConversionStrategy(),
            new NullableTypeConversionStrategy(this),
            new DictionaryTypeConversionStrategy(this),
            new ListTypeConversionStrategy(this)
        };
        
        var convertedType = Convert(token.Type);
        
        var _t = new TypescriptToken
        {
            Type = convertedType,
            Identifier = token.Identifier
        };

        return _t;
    }

    public string Convert(string type)
    {
        foreach (var _strategy in _typeHandlers)
        {
            if (_strategy.CanConvert(type))
               type = _strategy.Convert(type);
        }

        return type;
    }
    
    
}
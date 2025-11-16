namespace TokenGenerator.interfaces;

public interface ITokenGenerator
{
    TypescriptToken? InterpretToken(Token token);
    TypescriptToken ConvertType(TypescriptToken token); 
    TypescriptToken ConvertIdentifier(TypescriptToken token); 
}   
namespace TokenGenerator.interfaces;

public interface ITokenGenerator
{
    IParsedToken ConvertToken(IToken token);
    IParsedToken ConvertType(IParsedToken token); 
    IParsedToken ConvertIdentifier(IParsedToken token); 
}   
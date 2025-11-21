namespace TokenGenerator.interfaces;

internal interface ITokenHandler
{
    void Verify(TypescriptToken token);
    void Convert(TypescriptToken token);
}
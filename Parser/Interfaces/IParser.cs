using TokenGenerator.interfaces;

namespace Parser.Interfaces;

internal interface IParser
{
    Task<ConversionResult> Parse(List<IToken> rawTokens);
}